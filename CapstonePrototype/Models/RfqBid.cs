namespace CapstonePrototype.Models;
public class RfqBid
{
    public int Id {get;set;}
    public Rfq Rfq {get;set;} = null!;
    public Company Company {get;set;} = null!;
    public string BidStatus {get;set;} = null!;
    public string FileUrl {get;set;} = null!;
    public string Description {get;set;} = null!;
    public int Amount {get;set;} = 0;
    public DateTime OrderDeadline {get;set;}
    public DateTime ExpectedArrival {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}
}