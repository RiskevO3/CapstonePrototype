using CapstonePrototype.Dto.PurchasedProduct;

namespace CapstonePrototype.Dto.Rfq;
public class RfqResponseDto
{
    public int Id {get;set;}
    public string Title {get;set;} = null!;
    public string CompanyName {get;set;} = null!;
    public string Category {get;set;} = null!;
    public string BidType {get;set;} = null!;
    public string Description {get;set;} = null!;
    public List<PurchasedProductDto> PurchasedProducts {get;set;} = [];
    public DateTime OrderDeadline {get;set;}
    public DateTime ExpectedArrival {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}
}