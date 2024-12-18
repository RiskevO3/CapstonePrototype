using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CapstonePrototype.Data;
using CapstonePrototype.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace CapstonePrototype.Services.JwtService;
public class JwtService(IConfiguration configuration, ApplicationDbContext context) : IJwtService
{
  private readonly IConfiguration _configuration = configuration;
  private readonly ApplicationDbContext context = context;
  public JwtSecurityToken GenerateAccessToken(User user, bool isEmailVerified = false)
  {
    var authClaims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new("email", user.Email),
            new(ClaimTypes.Email, user.Email),
            new("firstName", user.FirstName),
            new("lastName", user.LastName),
            new("companyId", user.CompanyId.ToString()),
        };
    var token = CreateToken(authClaims);
    return token;
  }
  public async Task<JwtSecurityToken> GenerateRefreshToken(User user)
  {
    var newToken = GenerateRandomNumber();
    var searchUser = await context.Users.FirstOrDefaultAsync(x => x.Id == user.Id) ?? throw new Exception("User not found");
    var authClaims = new List<Claim>
        {
            new("token", newToken),
        };
    var token = CreateToken(authClaims, true);
    searchUser.RefreshToken = newToken;
    await context.SaveChangesAsync();
    return token;
  }
  public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
  {
    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidateAudience = false,
      ValidateIssuer = false,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? "secret")),
      ValidateLifetime = false
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
    if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
    {
      throw new SecurityTokenException("Invalid token");
    }
    return principal;
  }
  public string GenerateRandomNumber()
  {
    var randomNumber = new byte[64];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }
  private JwtSecurityToken CreateToken(List<Claim> authClaims, bool isRefreshToken = false)
  {
    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
    _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int RefreshTokenValidityInDays);
    var token = new JwtSecurityToken(
        issuer: _configuration["JWT:ValidIssuer"],
        audience: _configuration["JWT:ValidAudience"],
        expires: !isRefreshToken ? DateTime.Now.AddMinutes(tokenValidityInMinutes) : DateTime.Now.AddDays(RefreshTokenValidityInDays),
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    return token;
  }
  private static byte[] Generate256BitKey(string input)
  {
        return SHA256.HashData(Encoding.UTF8.GetBytes(input));
  }
  public SymmetricSecurityKey GetSigningKey()
  {
    // Generate a 256-bit key from the input string
    var secret = _configuration["JWT:Secret"] ?? "secret";
    var keyBytes = Generate256BitKey(secret);
    var authSigningKey = new SymmetricSecurityKey(keyBytes);
    return authSigningKey;
  }
}