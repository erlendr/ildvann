using System.Text.Json;

using IldVann.Embeddings;
using IldVann.Embeddings.Models;
using IldVann.Embeddings.Repositories;

using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var vmpRepository = new VmpRepository();
var products = vmpRepository.GetAllProducts();
Console.WriteLine($"Found {products.Count} VMP products");

List<VmpProductEmbedding> vmpProductEmbeddings = new();
var vectorEmbeddingGenerator = new VectorEmbeddingGenerator(configuration);

foreach (var product in products)
{
    Console.WriteLine($"Generating vector embedding for product '{product.Name}'");
    var nameVectorEmbedding = vectorEmbeddingGenerator.GenerateEmbedding(product.Name);

    vmpProductEmbeddings.Add(new VmpProductEmbedding
    {
        NameVectorEmbedding = nameVectorEmbedding, Code = product.Code
    });
}

// Serialize vmpProductEmbeddings to JSON and save to file
var json = JsonSerializer.Serialize(vmpProductEmbeddings);
await File.WriteAllTextAsync("vmpProductEmbeddings.json", json);