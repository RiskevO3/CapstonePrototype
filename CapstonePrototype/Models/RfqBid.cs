namespace CapstonePrototype.Models;
public class RfqBid
{
    public int Id {get;set;}
    public int RfqId {get;set;}
    public Rfq Rfq {get;set;} = null!;
    public int CompanyId {get;set;}
    public Company Company {get;set;} = null!;
    public int UserId {get;set;}
    public User User {get;set;} = null!;
    public string BidStatus {get;set;} = null!;
    public string Description {get;set;} = null!;
    public int Amount {get;set;} = 0;
    public string FileUrl {get;set;} = "";
    public string FilePaymentUrl {get;set;} = "";
    public string NoResi {get;set;} = "";

    public DateTime OrderDeadline {get;set;}
    public DateTime ExpectedArrival {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}
}