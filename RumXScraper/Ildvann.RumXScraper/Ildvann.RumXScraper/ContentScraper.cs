using System.Diagnostics;
using AngleSharp.Html.Parser;
using Ildvann.RumXScraper.Models;

namespace Ildvann.RumXScraper;

public class ContentScraper
{
    public async Task<Rum> GetRumByPageUrl(Uri url)
    {
        ArgumentNullException.ThrowIfNull(url);
        
        // Start stopwatch to measure time taken to download and parse HTML document
        var stopwatch = Stopwatch.StartNew();
        
        // Change domain from rum-x.fr to rum-x.com to download english content
        var englishUrl = new Uri("https://www.rum-x.com" + url.AbsolutePath);
        
        Console.WriteLine($"Downloading {englishUrl}...");
        
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(englishUrl);
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        var parser = new HtmlParser();
        var document = parser.ParseDocument(html);
        
        // Extract title, image, subtitle, rating, ratings, and RxId from HTML document

        var title = document.QuerySelector("span[data-pagefind-meta='basics_shortName']")?.TextContent;
        var subtitle = document.QuerySelector("span[data-pagefind-meta='basics_title']")?.TextContent;
        var img = document.QuerySelector("img[data-pagefind-meta='image[src]']")?.GetAttribute("src");
        var desc = document.QuerySelector("span[data-pagefind-meta='basics_description']")?.TextContent;
        var rating = double.Parse(document.QuerySelector("span[data-pagefind-meta='ratings_median']")?.TextContent ?? string.Empty) / 10.0;
        var ratings = int.Parse(document.QuerySelector("span[data-pagefind-meta='ratings_count']")?.TextContent ?? string.Empty);
        var rxid = document.QuerySelector("span[data-pagefind-meta='rxid']")?.TextContent;
        var country = document.QuerySelector("span[data-pagefind-meta='country_en']")?.TextContent;
        
        // Stop stopwatch and print time taken to download and parse HTML document
        stopwatch.Stop();
        Console.WriteLine($"Downloaded {englishUrl} and parsed in {stopwatch.ElapsedMilliseconds} ms");
        
        return new Rum
        {
            Title = title ?? throw new InvalidOperationException("Title not found"),
            Subtitle = subtitle ?? throw new InvalidOperationException("Subtitle not found"),
            Img = img ?? throw new InvalidOperationException("Image not found"),
            Rating = rating,
            Ratings = ratings,
            RxId = rxid ?? throw new InvalidOperationException("RxId not found"),
            Desc = desc ?? throw new InvalidOperationException("Description not found"),
            Country = country ?? throw new InvalidOperationException("Country not found")
        };
    }
}