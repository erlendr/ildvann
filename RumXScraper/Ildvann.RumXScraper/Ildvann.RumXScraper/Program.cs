using Ildvann.RumXScraper;
using Ildvann.RumXScraper.Models;
using Spectre.Console;



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

await AnsiConsole.Progress()
    .Columns(
        new TaskDescriptionColumn(),
        new SpinnerColumn()
        )
    .StartAsync(async ctx => 
    {
        // Define tasks
        var task1 = ctx.AddTask("[green]Scraping sitemap data[/]");

        while(!ctx.IsFinished) 
        {
            const string rumXSitemapUrl = "https://www.rum-x.fr/sitemap.xml";
            var sitemapScraper = new SitemapScraper();
            var rumXUrls = await sitemapScraper.ScrapeSitemapAsync(rumXSitemapUrl);
            
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
            
            // sort rums alphabetically
            rums.Sort((a, b) => string.Compare(a.AbsolutePath, b.AbsolutePath, StringComparison.Ordinal));
            task1.StopTask();
        }
    });

AnsiConsole.WriteLine($"Countries: {countries.Count}");
AnsiConsole.WriteLine($"Distilleries: {distilleries.Count}");
AnsiConsole.WriteLine($"Bottlers: {bottlers.Count}");
AnsiConsole.WriteLine($"Rums: {rums.Count}");
AnsiConsole.WriteLine($"Other: {other.Count}");

var contentScraper = new ContentScraper();
var scrapedRums = new List<Rum>();
var rumByPageUrls = await contentScraper.GetRumByPageUrls(rums.ToList());
scrapedRums.AddRange(rumByPageUrls);

var outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "rums.json");
await OutputWriter.WriteRumsToJsonAsync(scrapedRums, outputFilePath);

