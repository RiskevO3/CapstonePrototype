namespace CapstonePrototype.Dto;
public class PaginationDto
{
    public int Page {get;set;} = 0;
    public int PageSize {get; set;} = 0;
    public int TotalPage {get;set;} = 0;
    public int TotalData {get;set;} = 0;
}