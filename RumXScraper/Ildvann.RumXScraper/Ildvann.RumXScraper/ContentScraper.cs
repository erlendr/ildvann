using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Ildvann.RumXScraper.Models;

namespace Ildvann.RumXScraper;

public class ContentScraper
{
    public async Task<Rum> GetRumByPageUrl(Uri url, CancellationToken token = new())
    {
        ArgumentNullException.ThrowIfNull(url);

        // Change domain from rum-x.fr to rum-x.com to download english content
        var englishUrl = new Uri("https://www.rum-x.com" + url.AbsolutePath);

        Console.WriteLine($"Downloading {englishUrl}...");

        var html = await GetHtml(englishUrl, token);
        var parser = new HtmlParser();
        var document = await parser.ParseDocumentAsync(html, token);

        var rumByPageUrl = ParseDocumentToRum(document, englishUrl);
        return rumByPageUrl;
    }

    public async Task<List<Rum>> GetRumByPageUrls(List<Uri> urls)
    {
        ArgumentNullException.ThrowIfNull(urls);

        ParallelOptions parallelOptions = new()
        {
            MaxDegreeOfParallelism = 5
        };

        var rums = new List<Rum>();

        await Parallel.ForEachAsync(urls, parallelOptions, async (uri, token) =>
        {
            var rum = await GetRumByPageUrl(uri, token);
            rums.Add(rum);
            Console.WriteLine($"Parsed {rum.Title}");
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
        var country = document.QuerySelector("span[data-pagefind-meta='country_en']")?.TextContent;

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

    private static async Task<string> GetHtml(Uri englishUrl, CancellationToken token = new())
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(englishUrl, token);
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        return html;
    }
}