using System.Security.Claims;
using CucaLanches.Application.Users.DTOs;
using CucaLanches.Application.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CucaLanches.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController:ControllerBase
{
    
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<List<UserResponseDTO>>> ListAsync()
    {
        var result = await _userService.ListAsync();
        
      return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<UserResponseDTO>> CreateAsync(UserRequestDTO user)
    {
        var result = await _userService.CreateAsync(user);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var requestingUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var result = await _userService.DeleteAsync(id,requestingUserId);
        
        return Ok(result);
    }
}