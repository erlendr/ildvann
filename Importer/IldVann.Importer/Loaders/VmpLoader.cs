using System.Text.Json;

using Vinmonopolet.Models;

namespace IldVann.Importer.Loaders;

public class VmpLoader
{
    private JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new UnknownEnumConverter() }
    };

    public async Task<List<BaseProduct>> LoadProducts()
    {
        var productJsonFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!,
            "../../../../../VmpScraper/Ildvann.VmpScraper/Ildvann.VmpScraper/products.json");
        
        var productsJsonString = await File.ReadAllTextAsync(productJsonFile);
        var products =
            JsonSerializer.Deserialize<List<BaseProduct>>(productsJsonString, options: _jsonSerializerOptions);

        if (products == null)
        {
            Console.WriteLine("Failed to load VMP products");
            Environment.Exit(1);
        }

        return products;
    }
}