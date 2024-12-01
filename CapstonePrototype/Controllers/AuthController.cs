using CapstonePrototype.Dto;
using CapstonePrototype.Services.AuthService;
using Microsoft.AspNetCore.Mvc;
using SealBackend.Dto;
namespace CapstonePrototype.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService):ControllerBase
{
    private readonly IAuthService _authService = authService;
    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponse<RegisterResponseDto>>> Register(RegisterInputDto register)
    {
        var response = await _authService.Register(register);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ServiceResponse<LoginResponseDto>>> Login(LoginInputDto login)
    {
        var response = await _authService.Login(login);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }
}