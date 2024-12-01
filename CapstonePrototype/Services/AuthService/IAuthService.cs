using CapstonePrototype.Dto;
using CapstonePrototype.Models;
using SealBackend.Dto;

namespace CapstonePrototype.Services.AuthService;
public interface IAuthService
{
        public Task<ServiceResponse<RegisterResponseDto>> Register(RegisterInputDto register);
        public Task<ServiceResponse<LoginResponseDto>> Login(LoginInputDto login);
        public Task<User> GetAuthenticatedUser();
}