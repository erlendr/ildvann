using System.Text;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Ildvann.RumXScraper.Models;
using Spectre.Console;

namespace Ildvann.RumXScraper;

public partial class ContentScraper
{
    public async Task<Rum> GetRumByPageUrl(Uri url, CancellationToken token = new())
    {
        ArgumentNullException.ThrowIfNull(url);

        // Change domain from rum-x.fr to rum-x.com to download english content
        var englishUrl = new Uri("https://www.rum-x.com" + url.AbsolutePath);

        // Console.WriteLine($"Downloading {englishUrl}...");

        var html = await GetRawHtmlContent(englishUrl, token);

        var parser = new HtmlParser();
        var document = await parser.ParseDocumentAsync(html, token);

        var rumByPageUrl = ParseDocumentToRum(document, englishUrl);
        return rumByPageUrl;
    }

    public async Task<List<Rum>> GetRumByPageUrls(List<Uri> urls)
    {
        ArgumentNullException.ThrowIfNull(urls);

        var maxDegreeOfParallelism = 5;
        ParallelOptions parallelOptions = new()
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };

        var rums = new List<Rum>();

        await AnsiConsole.Progress()
            .Columns(
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new RemainingTimeColumn()
            )
            .StartAsync(async ctx =>
            {
                // Define tasks
                var task1 = ctx.AddTask($"[green]Scraping {urls.Count} rums (p: {maxDegreeOfParallelism})[/]");
                task1.MaxValue(urls.Count);

                while (!ctx.IsFinished)
                {
                    await Parallel.ForEachAsync(urls, parallelOptions, async (uri, token) =>
                    {
                        var rum = await GetRumByPageUrl(uri, token);
                        rums.Add(rum);
                        
                        // Increment
                        task1.Increment(1);
                    });
                }
            });
        

        return rums;
    }

    private static Rum ParseDocumentToRum(IHtmlDocument document, Uri englishUrl)
    {
        var title = document.QuerySelector("span[data-pagefind-meta='basics_shortName']")?.TextContent;
        var subtitle = document.QuerySelector("span[data-pagefind-meta='basics_title']")?.TextContent;
        var img = document.QuerySelector("img[data-pagefind-meta='image[src]']")?.GetAttribute("src");
        var desc = document.QuerySelector("span[data-pagefind-meta='basics_description']")?.TextContent;
        var rating =
            double.Parse(document.QuerySelector("span[data-pagefind-meta='ratings_median']")?.TextContent ??
                         string.Empty) / 10.0;
        var ratings = int.Parse(document.QuerySelector("span[data-pagefind-meta='ratings_count']")?.TextContent ??
                                string.Empty);
        var rxid = document.QuerySelector("span[data-pagefind-meta='rxid']")?.TextContent;
        var country = document.QuerySelector("div[itemprop='countryOfOrigin'] meta[itemprop='name']")?.GetAttribute("content");

        // count number of \u20AC characters in description
        var euroCount = desc?.Count(c => c == '\u20AC') ?? 0;

        // Remove \u20AC characters from description
        desc = desc?.Replace("\u20AC", string.Empty).Trim();

        return new Rum
        {
            Title = title ?? throw new InvalidOperationException("Title not found"),
            Subtitle = subtitle ?? throw new InvalidOperationException("Subtitle not found"),
            Img = img ?? throw new InvalidOperationException("Image not found"),
            Rating = rating,
            Ratings = ratings,
            RxId = rxid ?? throw new InvalidOperationException("RxId not found"),
            Desc = desc ?? throw new InvalidOperationException("Description not found"),
            Country = country ?? throw new InvalidOperationException("Country not found"),
            Url = englishUrl,
            PriceRange = euroCount
        };
    }

    private static async Task<string> GetRawHtmlContent(Uri englishUrl, CancellationToken token = new())
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(englishUrl, token);
        response.EnsureSuccessStatusCode();
        var buf = await response.Content.ReadAsByteArrayAsync(token);
        var content = Encoding.UTF8.GetString(buf);
        return content;
    }
}