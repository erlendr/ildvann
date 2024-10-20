using Ildvann.RumXScraper;
using Ildvann.RumXScraper.Models;

const string rumXSitemapUrl = "https://www.rum-x.fr/sitemap.xml";
var sitemapScraper = new SitemapScraper();
var rumXUrls = await sitemapScraper.ScrapeSitemapAsync(rumXSitemapUrl);

// Split list of URLs into lists according to the following rules:
// - If URL path starts with /countries/, add to countries list
// - If URL path starts with /distilleries/, add to distilleries list
// - If URL path starts with /bottlers/, add to bottlers list
// - If URL path starts with /rums/, add to rums list
// - Add all other URLs to other list

var countries = new List<Uri>();
var distilleries = new List<Uri>();
var bottlers = new List<Uri>();
var rums = new List<Uri>();
var other = new List<Uri>();

foreach (var url in rumXUrls)
{
    if (url.AbsolutePath.StartsWith("/countries/"))
    {
        countries.Add(url);
    }
    else if (url.AbsolutePath.StartsWith("/distilleries/"))
    {
        distilleries.Add(url);
    }
    else if (url.AbsolutePath.StartsWith("/bottlers/"))
    {
        bottlers.Add(url);
    }
    else if (url.AbsolutePath.StartsWith("/rums/") && !url.AbsolutePath.Equals("/rums/"))
    {
        rums.Add(url);
    }
    else
    {
        other.Add(url);
    }
}

Console.WriteLine($"Countries: {countries.Count}");
Console.WriteLine($"Distilleries: {distilleries.Count}");
Console.WriteLine($"Bottlers: {bottlers.Count}");
Console.WriteLine($"Rums: {rums.Count}");
Console.WriteLine($"Other: {other.Count}");

// sort rums alphabetically
rums.Sort((a, b) => string.Compare(a.AbsolutePath, b.AbsolutePath, StringComparison.Ordinal));

var contentScraper = new ContentScraper();

var scrapedRums = new List<Rum>();
scrapedRums.AddRange(await contentScraper.GetRumByPageUrls(rums.Take(1000).ToList()));

var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "rums.json");
await OutputWriter.WriteRumsToJsonAsync(scrapedRums, outputFilePath);

