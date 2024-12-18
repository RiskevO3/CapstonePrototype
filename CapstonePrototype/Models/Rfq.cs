
using CapstonePrototype.Dto.PurchasedProduct;
using CapstonePrototype.Dto.Rfq;

namespace CapstonePrototype.Models;
public class Rfq
{
    public int Id {get;set;}
    public int UserId {get;set;}
    public User User {get;set;} = null!;
    public string Title {get;set;} = null!;
    public Company Company {get;set;} = null!;
    public CompCategory CompCategory {get;set;} = null!;
    public string BidType {get;set;} = null!;
    public string Description {get;set;} = null!;
    public int Amount {get;set;} = 0;
    public DateTime OrderDeadline {get;set;}
    public DateTime ExpectedArrival {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}

    public RfqResponseDto AsDto(List<PurchasedProductDto> purchasedProducts)
    {
        return new RfqResponseDto
        {
            Id = Id,
            Title = Title,
            CompanyName = Company.Name,
            Category = CompCategory.AsRfqCategoryDto(),
            BidType = BidType,
            Description = Description,
            PurchasedProducts = purchasedProducts,
            OrderDeadline = OrderDeadline,
            ExpectedArrival = ExpectedArrival,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }

    public RfqItemDto AsItemDto()
    {
        return new RfqItemDto
        {
            Id = Id,
            Title = Title,
            CompanyName = Company.Name,
            Category = CompCategory.Name,
            Amount = Amount,
            OrderDeadline = OrderDeadline,
            ExpectedArrival = ExpectedArrival
        };
    }
    
}