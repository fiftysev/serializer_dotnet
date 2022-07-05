using System.Collections;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace SerializerNET;

public class Serializer
{
  public Serializer()
  {
  }

  public string Serialize(object obj)
  {
    return GetSerializedData(obj).ToString() ?? throw new InvalidOperationException();
  }

  public object Deserialize<T>(string s)
  {
    Type castTo = typeof(T);
    object obj = new();
    var props = castTo.GetProperties();
    GetDesearilizedData(s);
    return obj;
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

  private dynamic GetDesearilizedData(string s)
  {
    dynamic res = new ExpandoObject();

    throw new NotImplementedException();
  }

  private IEnumerable DeserializeArray(string s)
  {
    throw new NotImplementedException();
  }

  private IDictionary DeserializeDict(string s)
  {
    throw new NotImplementedException();
  }

  private object DeserializeClass(string s)
  {
    throw new NotImplementedException();
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
    var type = obj.GetType();
    var properties = type.GetProperties();
    var formattedProps = 
      properties
        .Select(property => $"\"{ToSnakeCase(property.Name)}\":" + $"{JsonValue(GetSerializedData(property.GetValue(obj) ?? "undefined"))}")
        .ToList();
    return $"{{\"type\":\"{type.Name}\", {string.Join(", ", formattedProps)}}}";
  }

  private static object JsonValue(object obj)
    => obj is string s && !s.StartsWith("{") ? $"\"{obj}\"" : obj;

  private static string ToSnakeCase(object s)
    => string.Join("_", Regex
      .Split(s as string ?? "", @"(?<!^)(?=[A-Z])")
      .Select(v => v.ToLower()));
}