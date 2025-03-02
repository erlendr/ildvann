using System.Text.Json;

using IldVann.Importer;

using Ildvann.Shared.Models;

namespace Ildvann.Shared.Loaders;

public class RumXLoader
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new UnknownEnumConverter() }
    };

    public async Task<List<RumXRum>> LoadRums()
    {
        var rumsJsonFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!,
            "../../../../../RumXScraper/Ildvann.RumXScraper/Ildvann.RumXScraper/rums.json");
        
        var rumsJsonString = await File.ReadAllTextAsync(rumsJsonFile);
        var rums =
            JsonSerializer.Deserialize<List<RumXRum>>(rumsJsonString, options: _jsonSerializerOptions);

        if (rums == null)
        {
            Console.WriteLine("Failed to load RumX rums");
            Environment.Exit(1);
        }

        return rums;
    }
}