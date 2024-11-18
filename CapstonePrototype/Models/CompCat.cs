namespace CapstonePrototype.Models;
public class CompCat
{
    public int Id {get;set;}
    public int CompanyId {get;set;}
    public Company Company {get;set;} = null!;
    public int CompCategoryId {get;set;}
    public CompCategory CompCategory {get;set;} = null!;
}