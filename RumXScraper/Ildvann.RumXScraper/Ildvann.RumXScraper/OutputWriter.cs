using System.Text.Json;
using Ildvann.RumXScraper.Models;

namespace Ildvann.RumXScraper;

public class OutputWriter
{
    // Take a list of Rum objects and write them to a JSON file using System.Text.Json
    public static async Task WriteRumsToJsonAsync(IEnumerable<Rum> rums, string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // Check if the file already exists and delete it if it does
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        await using var stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, rums, options);
        
        Console.WriteLine($"Wrote rum data to '{filePath}'");
    }
}