using System.Xml.Linq;

namespace Ildvann.RumXScraper;

public class SitemapScraper
{
    // A method that scrapes a sitemap and returns a list of URLs from it
    public async Task<List<string>> ScrapeSitemapAsync(string sitemapUrl)
    {
        var httpClient = new HttpClient();
        var sitemap = await httpClient.GetStringAsync(sitemapUrl);
        var doc = XDocument.Parse(sitemap);
        var ns = doc.Root?.GetDefaultNamespace() ?? throw new Exception("Invalid XML: XNamespace not found");
        return doc.Descendants(ns + "loc").Select(urlElement => urlElement.Value).ToList();
    }
}