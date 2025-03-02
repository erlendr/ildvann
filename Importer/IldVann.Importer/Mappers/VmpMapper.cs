using IldVann.Importer.Dtos;

using Vinmonopolet.Models;

namespace IldVann.Importer.Mappers;

public class VmpMapper
{
    public List<ProductDto> MapBaseProductToProductDto(List<BaseProduct> products)
    {
        List<ProductDto> productList = [];
        foreach (var product in products)
        {
            productList.Add(new ProductDto
            {
                Name = product.Name,
                Code = product.Code,
                Main_Country = product.MainCountry.Name,
                Main_Category_Name = product.MainCategory.Name,
                Main_Category_Code = product.MainCategory.Code,
                Main_Subcategory_Name = product.MainSubCategory.Name,
                Main_Subcategory_Code = product.MainSubCategory.Code,
                Product_Selection = product.ProductSelection.ToString()
            });
        }

        return productList;
    }
}