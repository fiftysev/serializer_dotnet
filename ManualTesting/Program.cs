using SerializerNET;

var serializer = new Serializer();

int[] arr = new[] { 1, 2, 3, 4 };

var person = new Person();

Console.WriteLine(serializer.Serialize(arr));

class Person
{
  public string Name { get; set; } = "Egor";
  public int Age { get; set; } = 19;
}

class Hobby
{
  public string HobbyName { get; set; } = "Music";
  public int Cost { get; set; } = 1230;
}