using System.Text.Json;
using System.IO;

namespace lbrry;

public static class StorageService
{
    private const string FilePath = "library.json";
    
    public static void Save(Dictionary<int, Book> books)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(books, options);
            File.WriteAllText(FilePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }
    
    public static Dictionary<int, Book> Load()
    {
        try
        {
            if (!File.Exists(FilePath)) 
                return new Dictionary<int, Book>();

            string jsonString = File.ReadAllText(FilePath);
            
            return JsonSerializer.Deserialize<Dictionary<int, Book>>(jsonString) ?? new Dictionary<int, Book>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
            return new Dictionary<int, Book>();
        }
    }
}