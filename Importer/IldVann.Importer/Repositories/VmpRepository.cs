using System.Data;
using System.Data.SqlClient;

using IldVann.Importer.Dtos;
using IldVann.Importer.Mappers;

using Npgsql;

using Vinmonopolet.Models;

using Z.Dapper.Plus;

namespace Ildvann.Importer.Repositories;

public class VmpRepository
{
    private readonly VmpMapper _mapper;

    public VmpRepository(VmpMapper vmpMapper)
    {
        _mapper = vmpMapper ?? throw new ArgumentNullException(nameof(vmpMapper));
    }

    public void BulkInsertProducts(List<BaseProduct> products)
    {
        var productList = _mapper.MapBaseProductToProductDto(products);

        if (productList.Count == 0)
        {
            Console.WriteLine("No products to insert, mapping failed");
            return;
        }

        using var connection = DbConnection.CreateConnection();
        connection
            .UseBulkOptions(options =>
            {
                options.InsertIfNotExists = true;
                options.DestinationTableName = "products";
            })
            .BulkInsert(productList);

        Console.WriteLine("Inserted {0} products", productList.Count);
    }
}