using System.ComponentModel.DataAnnotations;

namespace CapstonePrototype.Dto.PurchasedProduct;
public class PurchasedProductInsertDto
{
    [Required(ErrorMessage = "Product id is required")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Rfq id is required")]
    public int RfqId { get; set; }
    
    [Required(ErrorMessage ="Unit price is required")]
    public int UnitPrice { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    public int Amount { get; set; }
}