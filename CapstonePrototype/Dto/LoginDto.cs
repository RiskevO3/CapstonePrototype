using System.ComponentModel.DataAnnotations;

namespace CapstonePrototype.Dto;
public class LoginInputDto
{
    [EmailAddress(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [MinLength(6, ErrorMessage = "Password must be at least 6 characters"),MaxLength(20, ErrorMessage = "Password must be at most 20 characters")]
    public string Password { get; set; } = null!;
}

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}