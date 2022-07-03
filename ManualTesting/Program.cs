using System.Text.Json;
using SerializerNET;

var serializer = new Serializer();

int[] arr = new[] { 1, 2, 3, 4 };

var person = new Person{};

List<string> arr2 = new() { "Hello", "World" };

var res = serializer.Serialize(arr);

Console.WriteLine(res);

class Person
{
  public string Name { get; set; } = "Egor";
  public int Age { get; set; } = 19;
  public Hobby Hobby { get; set; } = new() { HobbyName = "asd", Cost = 123 };
}

struct Hobby
{
  public string HobbyName { get; set; }
  public int Cost { get; set; }
}