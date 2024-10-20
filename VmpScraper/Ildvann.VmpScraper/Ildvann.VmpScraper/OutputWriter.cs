using System.Text.Json;
using Vinmonopolet.Models;

namespace Ildvann.VmpScraper;

public class OutputWriter
{
    public static async Task WriteBaseProductsToJsonAsync(IEnumerable<BaseProduct> rums, string filePath)
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
        
        Console.WriteLine($"Wrote BaseProduct data to '{filePath}'");
    }
}