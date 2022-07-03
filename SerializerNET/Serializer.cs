using System.Collections;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;

namespace SerializerNET;

public class Serializer
{
  public Serializer() {}

  public string Serialize(object obj)
  {
    var type = obj.GetType();
    if (type.IsArray) return SerializeArray(obj as Array ?? Array.Empty<string>());
    if (obj is IDictionary dictionary) return SerializeDict(dictionary);
    if (type.IsClass && obj is not string) return SerializeClass(obj, type);
    return obj.ToString() ?? "";
  }

  public object Deserialize(string obj)
  {
    return new object();
  }


  private string SerializeArray(Array arr)
  {
    JsonArray jsonArray = new JsonArray();
    foreach (var v in arr) jsonArray.Add(v);
    return jsonArray.ToJsonString();
  }

  private string SerializeDict(IDictionary? dict)
  {
    return "42";
  }

  private string SerializeClass(object obj, Type type)
  {
    return "5";
  }
}