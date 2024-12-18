using CapstonePrototype.Dto.Category;
using CapstonePrototype.Dto.Rfq;

namespace CapstonePrototype.Models;
public class CompCategory
{
    public int Id {get;set;}
    public string Name {get;set;} = null!;
    public string Description {get;set;} = null!;

    public CategoryResultDto AsDto()
    {
        return new CategoryResultDto
        {
            Id = Id,
            Name = Name,
            Description = Description
        };
    }

    public RfqCategory AsRfqCategoryDto()
    {
        return new RfqCategory
        {
            Id = Id,
            Name = Name
        };
    }
}