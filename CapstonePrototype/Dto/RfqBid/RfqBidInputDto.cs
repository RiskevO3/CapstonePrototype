namespace CapstonePrototype.Dto.RfqBid;
public class RfqBidInputDto
{
    public int RfqId {get;set;}
    public int Amount {get;set;}
    public string Status {get;set;} = "";
    public string Description {get;set;} = "";
    public string FileUrl {get;set;} = "";
    public string FilePaymentUrl {get;set;} = "";
    public string Resi {get;set;} = "";
    public bool IsCompleted {get;set;}
    public DateTime OrderDeadline {get;set;}
    public DateTime ExpectedArrival {get;set;}
}