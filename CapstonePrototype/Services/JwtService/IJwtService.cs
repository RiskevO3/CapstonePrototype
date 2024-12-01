using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CapstonePrototype.Models;

namespace CapstonePrototype.Services.JwtService;
public interface IJwtService
{
    public JwtSecurityToken GenerateAccessToken(User user, bool isEmailVerified=false);
    public Task<JwtSecurityToken> GenerateRefreshToken(User user);
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    public string GenerateRandomNumber();
}