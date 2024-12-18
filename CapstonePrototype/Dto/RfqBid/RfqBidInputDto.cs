namespace CapstonePrototype.Dto.RfqBid;
public class RfqBidInputDto
{
    public int RfqId {get;set;}
    public int Amount {get;set;}
    public string Description {get;set;} = "";
    public DateTime OrderDeadline {get;set;}
    public DateTime ExpectedArrival {get;set;}
}