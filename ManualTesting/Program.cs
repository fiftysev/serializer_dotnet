using SerializerNET;

var serializer = new Serializer();

Dictionary<string, string> dict = new();

Console.WriteLine(serializer.Serialize(dict));
Console.WriteLine(serializer.Serialize("Hello"));
Console.WriteLine(serializer.Serialize(new int[]{1, 2, 3, 4}));
Console.WriteLine(serializer.Serialize(1));
