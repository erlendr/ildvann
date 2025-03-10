namespace Ildvann.Shared.Models;

public record struct RumXRum
{
    public string Title { get; set; }
    public string Img { get; set; }
    public string Subtitle { get; set; }
    public double Rating { get; set; }
    public int Ratings { get; set; }
    public string RxId { get; set; }
    public string Desc { get; set; }
    public string Country { get; set; }
    public Uri Url { get; set; }
    public int PriceRange { get; set; }
}