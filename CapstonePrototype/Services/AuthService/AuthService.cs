using CapstonePrototype.Data;

namespace CapstonePrototype.Services.AuthService;
public class AuthService(ApplicationDbContext context):IAuthService
{
    private readonly ApplicationDbContext _context = context;
}