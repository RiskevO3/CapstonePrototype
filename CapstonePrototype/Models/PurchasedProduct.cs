using CapstonePrototype.Dto.PurchasedProduct;

namespace CapstonePrototype.Models;
public class PurchasedProduct
{
    public int Id {get;set;}
    public Product Product {get;set;} = null!;
    public Rfq Rfq {get;set;} = null!;
    public int UnitPrice {get;set;}
    public int Quantity {get;set;}
    public int Amount {get;set;}

    public PurchasedProductDto AsDto()
    {
        return new PurchasedProductDto
        {
            Id = Id,
            Name = Product.Name,
            Image = Product.ImageUrl,
            UnitPrice = UnitPrice,
            Quantity = Quantity,
            Amount = Amount
        };
    }
}