using System.ComponentModel.DataAnnotations;

namespace CapstonePrototype.Dto.Product;
public class ProductInsertDto
{
    public string? Image { get; set; } = null!;

    [Required(ErrorMessage = "Name is required"), MaxLength(50, ErrorMessage = "Name must be at most 50 characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Description is required"), MaxLength(500, ErrorMessage = "Description must be at most 500 characters")]
    public string Description { get; set; } = null!;
}