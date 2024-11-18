namespace CapstonePrototype.Models;
public class PurchasedProduct
{
    public int Id {get;set;}
    public Product Product {get;set;} = null!;
    public Rfq Rfq {get;set;} = null!;
    public int Quantity {get;set;}
    public int Amount {get;set;}
}