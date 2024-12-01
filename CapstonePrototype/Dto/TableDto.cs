namespace CapstonePrototype.Dto;
public class TableDto<T>
{
    public PaginationDto Pagination {get;set;} = new();
    public List<T> Data {get;set;} = [];
}