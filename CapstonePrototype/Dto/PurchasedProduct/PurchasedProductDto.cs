namespace CapstonePrototype.Dto.PurchasedProduct;
public class PurchasedProductDto
{
    public int Id {get;set;}
    public string Name {get;set;} = null!;
    public string Image {get;set;} = null!;
    public string Description {get;set;} = null!;
    public int UnitPrice {get;set;}
    public int Quantity {get;set;}
    public int Amount {get;set;}
}