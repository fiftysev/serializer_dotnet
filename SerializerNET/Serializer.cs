using System.Collections;

namespace SerializerNET;

public class Serializer
{
  public Serializer() {}

  public string Serialize(object obj)
  {
    return GetSerializedData(obj).ToString() ?? throw new InvalidOperationException();
  }

  public object Deserialize(string obj)
  {
    return new object();
  }

  private object GetSerializedData(object obj)
  {
     var type = obj.GetType();
     if (type.IsArray)
     {
       return SerializeArray(obj as Array ?? Array.Empty<string>());
     }
 
     if (obj is IDictionary dictionary)
     {
       return SerializeDict(dictionary);
     }
 
     if (type.IsClass && obj is not string && !type.IsGenericType)
     {
       return SerializeClass(obj);
     }

     return obj ?? "";
  }


  private string SerializeArray(Array arr)
  {
    List<string> formattedArr = new();
    foreach (var v in arr) formattedArr.Add($"{JsonValue(GetSerializedData(v))}");
    return $"[{string.Join(",", formattedArr)}]";
  }

  private string SerializeDict(IDictionary dictionary)
  {
    List<string> formattedProps = new();
    foreach (DictionaryEntry v in dictionary)
    {
      formattedProps.Add($"{{\"{v.Key}\": {JsonValue(GetSerializedData(v.Value ?? "undefined"))}}}");
    }
    return $"{{{string.Join(",", formattedProps)}}}";
  }

  private string SerializeClass(object obj)
  {
    var type = obj.GetType();
    var properties = type.GetProperties();
    List<string> formattedProps = new();
    foreach (var property in properties)
    {
      formattedProps.Add($"\"{property.Name.ToLower()}\":" +
                         $"{JsonValue(GetSerializedData(property.GetValue(obj) ?? "undefined"))}");
      Console.WriteLine(property.GetValue(obj) is string);
    }
    return $"{{\"type\":\"class\", {string.Join(", ", formattedProps)}}}";
  }

  private static object JsonValue(object obj) => obj is string ? $"\"{obj}\"" : obj;
  }