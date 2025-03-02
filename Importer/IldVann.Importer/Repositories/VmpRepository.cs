using System.Data;
using System.Data.SqlClient;

using IldVann.Importer.Dtos;

using Npgsql;

using Vinmonopolet.Models;

using Z.Dapper.Plus;

namespace Ildvann.Importer.Repositories;

public class VmpRepository
{
    public VmpRepository()
    {
    }

    private IDbConnection CreateConnection()
    {
        const string server = "localhost";
        const string database = "postgres";
        const string userId = "erlend";
        const string password = "postgres";

        const string connectionString = $"Host={server};Port=5432;Database={database};Username={userId};Password={password}";
        return new NpgsqlConnection(connectionString);
    }

    public void BulkInsertProducts(List<BaseProduct> products)
    {
        // Map BaseProduct to DTO product
        List<ProductDto> productList = [];
        foreach (var product in products)
        {
            if (string.IsNullOrEmpty(product.Code))
            {
                continue;
            }
            
            productList.Add(new ProductDto
            {
                Name = product.Name,
                Code = product.Code,
                Main_Country = product.MainCountry.Name,
                Main_Category_Name = product.MainCategory.Name,
                Main_Category_Code = product.MainCategory.Code,
                Main_Subcategory_Name = product.MainSubCategory.Name,
                Main_Subcategory_Code = product.MainSubCategory.Code,
                Product_Selection = product.ProductSelection.ToString()
            });
        }

        if (productList.Count == 0)
        {
            Console.WriteLine("No products to insert, mapping failed");
            return;
        }

        Console.WriteLine("Mapped {0} products", productList.Count);

        using var connection = CreateConnection();
        connection
            .UseBulkOptions(options =>
            {
                options.InsertIfNotExists = true;
                options.DestinationTableName = "products";
            })
            .BulkInsert(productList);
    }
}