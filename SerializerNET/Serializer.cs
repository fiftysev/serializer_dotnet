using System.Collections;
using System.Text.RegularExpressions;

namespace SerializerNET;

public class Serializer
{
  public string Serialize(object obj)
  {
    return GetSerializedData(obj).ToString() ?? throw new InvalidOperationException();
  }
  
  public object Deserialize(string s)
  {
    if (s.StartsWith("{") && s.EndsWith("}")) return DeserializeKeyValue(s);
    if (s.StartsWith("[") && s.EndsWith("]")) return DeserializeArray(s);
    return DeserializePrimitive(s);
  }

  private object GetSerializedData(object obj)
  {
    var type = obj.GetType();
    if (obj is IDictionary dictionary) return SerializeDict(dictionary);
    if (type.IsArray || (obj is IEnumerable && type.IsGenericType))
      return SerializeArray(obj as IEnumerable ?? Array.Empty<string>());
    if (type.IsClass && obj is not string && !type.IsGenericType || type.IsValueType && !type.IsPrimitive)
      return SerializeClass(obj);
    return obj ?? "";
  }

  private IEnumerable DeserializeArray(string s)
  {
    var rawArray = s.Trim('[', ']').Split(",").ToList();
    return (from string? v in rawArray select Serialize(v)).ToList();
  }

  private object DeserializeKeyValue(string s)
  {
    Hashtable res = new();
    var rawDict = s.Trim('\n', '{', '}').Split(",").Where(v => !v.Equals("\"type\":\"class\""));
    foreach (var pair in rawDict.Select(v => v.Split(':')))
    {
      res[pair[0]] = Serialize(pair[1]);
    }
    return res;
  }

  private object DeserializePrimitive(string s)
  {
    if (double.TryParse(s, out var num)) return num;
    if (bool.TryParse(s, out var boolVar)) return boolVar;
    return s.Trim('"');
  }

  private string SerializeArray(IEnumerable arr)
  {
    List<string> formattedArr = (from object? v in arr select $"{JsonValue(GetSerializedData(v))}").ToList();
    return $"[{string.Join(",", formattedArr)}]";
  }

  private string SerializeDict(IDictionary dictionary)
  {
    List<string> formattedProps =
      (from DictionaryEntry v
          in dictionary
        select $"\"{ToSnakeCase(v.Key)}\": {JsonValue(GetSerializedData(v.Value ?? "undefined"))}").ToList();
    return $"{{{string.Join(",", formattedProps)}}}";
  }

  private string SerializeClass(object obj)
  {
    var properties = obj.GetType().GetProperties();
    var formattedProps =
      properties
        .Select(property => $"\"{ToSnakeCase(property.Name)}\":" +
                            $"{JsonValue(GetSerializedData(property.GetValue(obj) ?? "undefined"))}")
        .ToList();
    return $"{{\"type\":\"class\", {string.Join(", ", formattedProps)}}}";
  }

  private static object JsonValue(object obj)
    => obj is string s && !s.StartsWith("{") ? $"\"{obj}\"" : obj;

  private static string ToSnakeCase(object s)
    => string.Join("_", Regex
      .Split(s as string ?? "", @"(?<!^)(?=[A-Z])")
      .Select(v => v.ToLower()));
}