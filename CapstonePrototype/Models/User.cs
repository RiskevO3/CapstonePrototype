namespace CapstonePrototype.Models;
public class User
{
    public int Id { get; set; }
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
}