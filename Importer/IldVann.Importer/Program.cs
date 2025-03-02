using System.Text.Json;
using System.Text.Json.Serialization;

using IldVann.Importer;
using IldVann.Importer.Repositories;

using Vinmonopolet.Models;
using Vinmonopolet.Parsers;

var x = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

var filePath = Path.Combine(x!,
    "../../../../../VmpScraper/Ildvann.VmpScraper/Ildvann.VmpScraper/products.json");
var json = await File.ReadAllTextAsync(filePath);
// parse json with camelCase
JsonSerializerOptions jsonSerializerOptions = new()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Converters = { new UnknownEnumConverter() }
};
var products =
    JsonSerializer.Deserialize<List<BaseProduct>>(json, options: jsonSerializerOptions);
if (products == null)
{
    Console.WriteLine("Failed to load VMP products");
    Environment.Exit(1);
}

Console.WriteLine($"Found {products.Count} VMP products");

var vmpRepository = new VmpRepository();
vmpRepository.BulkInsertProducts(products);