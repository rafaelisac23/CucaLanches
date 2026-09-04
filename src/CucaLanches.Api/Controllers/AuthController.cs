using CucaLanches.Application.Auth.DTOs;
using CucaLanches.Application.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CucaLanches.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController:ControllerBase
{
    
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(UserLoginRequestDTO request)
    {
        var result = await _authService.LoginUserAsync(request);
        return Ok(result);
    }
    
    [HttpPost("client-login")]
    public async Task<IActionResult> LoginClient(ClientLoginRequestDTO request)
    {
        var result = await _authService.LoginClientAsync(request);
        return Ok(result);
    }
    
}