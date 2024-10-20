using Ildvann.RumXScraper;

const string rumXSitemapUrl = "https://www.rum-x.fr/sitemap.xml";
var sitemapScraper = new SitemapScraper();
var rumXUrls = await sitemapScraper.ScrapeSitemapAsync(rumXSitemapUrl);

foreach (var url in rumXUrls)
{
    Console.WriteLine(url);
}