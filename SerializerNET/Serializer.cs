using System.Collections;
using System.Runtime.CompilerServices;

namespace SerializerNET;

public class Serializer
{
  public Serializer() {}

  public string Serialize(object obj)
  {
    var type = obj.GetType();
    if (type.IsArray) return SerializeArray(obj as IEnumerable);
    if (obj is IDictionary dictionary) return SerializeDict(dictionary);
    if (type.IsClass && obj is not string) return SerializeClass(obj, type);
    return obj.ToString() ?? "";
  }

  public object Deserialize(string obj)
  {
    return new object();
  }


  private string SerializeArray(IEnumerable? arr)
  {
    return "43";
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