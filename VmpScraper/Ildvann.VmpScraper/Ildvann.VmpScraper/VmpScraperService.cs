using Microsoft.Extensions.Logging;
using Vinmonopolet;
using Vinmonopolet.Models;
using Vinmonopolet.Models.Facets.MainCategory;
using Vinmonopolet.Models.Facets.MainSubCategory;
using Vinmonopolet.QueryBuilder;

namespace Ildvann.VmpScraper;

public class VmpScraperService(ILogger<VmpScraperService> logger, IVinmonopoletClient vmpClient)
{
    private readonly ILogger<VmpScraperService> _logger = logger;

    public async Task Run()
    {
        var query = new ProductQueryBuilder()
            .MainCategory(MainCategoryFacets.Liquor)
            .MainSubCategory(MainSubCategoryFacets.Rum);

        var productQueryBuilderResult = query.Build();
        
        var res =  await vmpClient.GetProducts(productQueryBuilderResult, new PaginationOptions
        {
            CurrentPage = 0,
            PageSize = 50
        });
        LogResult(res);
        
        List<BaseProduct> baseProducts = [];
        baseProducts.AddRange(res.Products);
        
        while (res.Pagination.CurrentPage <= res.Pagination.TotalPages)
        {
            res = await res.NextPage(vmpClient);
            LogResult(res);
            baseProducts.AddRange(res.Products);
            Console.WriteLine("Added {0} products", res.Products.Count);
        }     
        
        Console.WriteLine("Total products: {0}", baseProducts.Count);
        await OutputWriter.WriteBaseProductsToJsonAsync(baseProducts, "products.json");
    }

    private static void LogResult(GetProductsResponse res)
    {
        Console.WriteLine("Total results: {0}", res.Pagination.TotalResults);
        Console.WriteLine("Page size: {0}", res.Pagination.PageSize);
        Console.WriteLine("Current page: {0}", res.Pagination.CurrentPage);
        Console.WriteLine("Total pages: {0}", res.Pagination.TotalPages);
        Console.WriteLine("Current query: '{0}'", res.CurrentQuery.Query.Value);
    }
}