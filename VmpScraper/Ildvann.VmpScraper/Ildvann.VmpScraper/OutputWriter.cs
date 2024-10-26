using System.Text.Json;
using System.Text.Json.Serialization;
using Vinmonopolet.Models;

namespace Ildvann.VmpScraper;

public class OutputWriter
{
    public static async Task WriteBaseProductsToJsonAsync(IEnumerable<BaseProduct> rums, string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter()
            }
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