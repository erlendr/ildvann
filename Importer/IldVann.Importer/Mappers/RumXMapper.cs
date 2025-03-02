using IldVann.Importer.Dtos;

using Ildvann.Shared.Models;

namespace IldVann.Importer.Mappers;

public class RumXMapper
{
    public List<RumXRumDto> MapToDto(List<RumXRum> rums)
    {
        List<RumXRumDto> rumDtos = [];

        foreach (var rum in rums)
        {
            rumDtos.Add(new RumXRumDto
            {
                Title = rum.Title,
                Img = rum.Img,
                Subtitle = rum.Subtitle,
                Rating = rum.Rating,
                Ratings = rum.Ratings,
                RxId = rum.RxId,
                Description = rum.Desc,
                Country = rum.Country,
                Url = rum.Url.ToString(),
                PriceRange = rum.PriceRange
            });
        }

        return rumDtos;
    }
}