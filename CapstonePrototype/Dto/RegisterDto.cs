using System.ComponentModel.DataAnnotations;

namespace CapstonePrototype.Dto;
public class RegisterInputDto
{
    [EmailAddress(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [MinLength(6, ErrorMessage = "Password must be at least 6 characters"),MaxLength(20, ErrorMessage = "Password must be at most 20 characters")]
    public string Password { get; set; } = null!;
    
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Company name id is required")]
    public int CompanyId { get; set; }
}

public class RegisterResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}