
namespace CapstonePrototype.Models;
public class Rfq
{
    public int Id {get;set;}
    public Company Company {get;set;} = null!;
    public CompCategory CompCategory {get;set;} = null!;
    public string BidType {get;set;} = null!;
    public string Description {get;set;} = null!;
    public DateTime OrderDeadline {get;set;}
    public DateTime ExpectedArrival {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}
}