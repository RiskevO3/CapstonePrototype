namespace CapstonePrototype.Models;
public class Company
{
    public int Id {get;set;}
    public Guid UniqueId {get;set;}
    public string Name {get;set;} = null!;
    public string Address {get;set;} = null!;
}