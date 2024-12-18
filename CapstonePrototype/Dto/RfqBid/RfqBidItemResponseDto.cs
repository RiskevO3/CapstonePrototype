namespace CapstonePrototype.Dto.RfqBid;
public class RfqBidItemResponseDto
{
    public int Id {get;set;}
    public string Name {get;set;} = "";
    public string CompanyName {get;set;} = "";
    public string Category {get;set;} = "";
    public string Status {get;set;} = "";
    public DateTime OrderDeadline {get;set;}
    public DateTime ExpectedArrival {get;set;}
    public string BidStatus {get;set;} = "";
    public int Amount {get;set;} = 0;
}