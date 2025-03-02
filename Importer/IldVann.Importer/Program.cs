using IldVann.Importer.Loaders;
using IldVann.Importer.Mappers;

using Ildvann.Importer.Repositories;

var vmpLoader = new VmpLoader();
var products = await vmpLoader.LoadProducts();
Console.WriteLine($"Found {products.Count} VMP products");

var vmpMapper = new VmpMapper();
var vmpRepository = new VmpRepository(vmpMapper);
vmpRepository.BulkInsertProducts(products);

var rumXLoader = new RumXLoader();
var rums = await rumXLoader.LoadRums();

var rumXMapper = new RumXMapper();
var rumXRepository = new RumXRepository(rumXMapper);

rumXRepository.BulkInsertRums(rums);