using Dapper;

using IldVann.Importer.Dtos;

using Ildvann.Shared;

namespace IldVann.Embeddings.Repositories;

public class VmpRepository
{
    public List<ProductDto> GetAllProducts()
    {
        using var connection = DbConnection.CreateConnection();
        
        return connection.Query<ProductDto>("SELECT * FROM products").ToList();
    }
}