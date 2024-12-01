namespace CapstonePrototype.Dto.Rfq;
public class RfqItemDto
{
    public int Id {get;set;}
    public string Title {get;set;}=null!;
    public string CompanyName {get;set;} = null!;
    public string Category {get;set;} = null!;
    public int Amount {get;set;} = 0;
    public DateTime OrderDeadline {get;set;}
    public DateTime ExpectedArrival {get;set;}
}