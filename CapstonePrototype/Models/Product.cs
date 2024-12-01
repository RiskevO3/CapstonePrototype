using CapstonePrototype.Dto.Product;

namespace CapstonePrototype.Models;
public class Product
{
    public int Id {get;set;}
    public string ImageUrl {get;set;} = null!;
    public string Name {get;set;} = null!;
    public string Description {get;set;} = null!;
    public int CompanyId {get;set;}
    public Company Company {get;set;} = null!;
    public ProductDto AsDto()
    {
        return new ProductDto
        {
            Id = Id,
            Image = ImageUrl,
            Name = Name,
            Description = Description
        };
    }
}