using Ildvann.VmpScraper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vinmonopolet.ServiceCollectionExtensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
{
    services.AddVinmonopolet();
    services.AddTransient<VmpScraperService>();
}).Build();

var vmpScraperService = host.Services.GetRequiredService<VmpScraperService>();
await vmpScraperService.Run();