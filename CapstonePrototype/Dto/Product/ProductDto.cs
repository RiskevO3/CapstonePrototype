namespace CapstonePrototype.Dto.Product;
public class ProductDto
{
    public int Id { get; set; }
    public string Image { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}