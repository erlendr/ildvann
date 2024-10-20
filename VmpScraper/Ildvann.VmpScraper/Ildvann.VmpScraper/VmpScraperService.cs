using System.Text.Json;
using Microsoft.Extensions.Logging;
using Vinmonopolet;
using Vinmonopolet.QueryBuilder;

namespace Ildvann.VmpScraper;

public class VmpScraperService(ILogger<VmpScraperService> logger, IVinmonopoletClient vinmonopoletClient)
{
    private readonly ILogger<VmpScraperService> _logger = logger;
    private readonly IVinmonopoletClient _vinmonopoletClient = vinmonopoletClient;

    public async Task Run()
    {
        var query = new ProductQueryBuilder();
        var res =  await _vinmonopoletClient.GetProducts(query.Build());
        Console.WriteLine("Got {0} products", res.Products.Count);
        
        await OutputWriter.WriteBaseProductsToJsonAsync(res.Products, "products.json");
    }
}