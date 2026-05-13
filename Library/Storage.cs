using System.Collections.Generic;
using System.Text.Json;

namespace lbrry;

public interface IStorage
{
    void Save(Dictionary<int, Book> books);
    Dictionary<int, Book> Load();
}

public class JsonStorageService : IStorage
{
    private readonly string _filePath;
    public JsonStorageService(string filePath = "library.json")
    {
        _filePath = filePath;
    }

    public void Save(Dictionary<int, Book> books)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(books, options);
        File.WriteAllText(_filePath, jsonString);
    }

    public Dictionary<int, Book> Load()
    {
        if (!File.Exists(_filePath)) 
            return new Dictionary<int, Book>();
        
        string jsonString = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<Dictionary<int, Book>>(jsonString) ?? new Dictionary<int, Book>();
    }
}