using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CapstonePrototype.Data;
using CapstonePrototype.Dto;
using CapstonePrototype.Models;
using CapstonePrototype.Services.JwtService;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using SealBackend.Dto;

namespace CapstonePrototype.Services.AuthService;
public class AuthService(ApplicationDbContext context,IJwtService jwtService,IHttpContextAccessor httpContextAccessor):IAuthService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


    public async Task<ServiceResponse<RegisterResponseDto>>Register(RegisterInputDto register)
    {
        try
        {
            var companyExist = await _context.Companies.FirstOrDefaultAsync(c => c.Id == register.CompanyId);
            if(companyExist == null)return new ServiceResponse<RegisterResponseDto>{Success = false, Message = "Company does not exist"};
            var user = new User
            {
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Company = companyExist,
                RefreshToken = _jwtService.GenerateRandomNumber(),
                Password = GenerateHashPassword(register.Password)
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = await _jwtService.GenerateRefreshToken(user);
            return new ServiceResponse<RegisterResponseDto>
            {
                Data = new RegisterResponseDto
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken)
                },
                Success = true,
                Message = "User registered successfully"
            };
        }
        catch(Exception e)
        {
            Console.WriteLine("Error: "+e.Message);
            return new ServiceResponse<RegisterResponseDto>
            {
                Success = false,
                Message = e.Message
            };
        }
    }
    public async Task<ServiceResponse<LoginResponseDto>> Login(LoginInputDto login)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if(user == null)return new ServiceResponse<LoginResponseDto>{Success = false, Message = "User not found"};
            if(!VerifyPassword(login.Password,user.Password))return new ServiceResponse<LoginResponseDto>{Success = false, Message = "Invalid password"};
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = await _jwtService.GenerateRefreshToken(user);
            return new ServiceResponse<LoginResponseDto>
            {
                Data = new LoginResponseDto
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken)
                },
                Success = true,
                Message = "User logged in successfully"
            };
        }
        catch(Exception e)
        {
            Console.WriteLine("Error From Login: "+e.Message);
            return new ServiceResponse<LoginResponseDto>
            {
                Success = false,
                Message = e.Message
            };
        }
    }
    public async Task<User> GetAuthenticatedUser()
    {
        var email = _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;
        if (email == null) return null!;
        return await _context.Users.FirstOrDefaultAsync(u => u.Email != null && u.Email == email) ?? null!;
    }
    private static string GenerateHashPassword(string password)
    {
        // Convert the string "secret" to a byte array to use as the salt
        byte[] salt = Encoding.UTF8.GetBytes("secret");
        // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        return hashed;
    }
    private static bool VerifyPassword(string inputPassword,string hashedPassword)
    {
        return hashedPassword == GenerateHashPassword(inputPassword);
    }
}