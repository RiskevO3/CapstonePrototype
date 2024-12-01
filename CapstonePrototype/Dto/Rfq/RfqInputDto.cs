using System.ComponentModel.DataAnnotations;
using CapstonePrototype.Dto.PurchasedProduct;

namespace CapstonePrototype.Dto.Rfq;
public class RfqInputDto
{
    [Required(ErrorMessage = "Comp category id is required")]
    public int CompCategoryId {get;set;}
    
    [Required(ErrorMessage = "Title is required")]
    public string Title {get;set;} = null!;
    
    [Required(ErrorMessage = "Bid type is required")]
    public string BidType {get;set;} = null!;

    [Required(ErrorMessage = "Description is required")]
    public string Description {get;set;} = null!;

    [Required(ErrorMessage = "Order deadline is required")]
    public DateTime OrderDeadline {get;set;}

    [Required(ErrorMessage = "Expected arrival is required")]
    public DateTime ExpectedArrival {get;set;}

    [Required(ErrorMessage = "Purchased products are required")]
    public List<PurchasedProductInsertDto> PurchasedProducts {get;set;} = [];
}