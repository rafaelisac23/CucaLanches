using CucaLanches.Application.PublicMenu.DTOs;
using CucaLanches.Application.PublicMenu.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CucaLanches.Api.Controllers;


[ApiController]
[Route("[controller]")]
public class PublicMenuController:ControllerBase
{
    private readonly IPublicMenuService _publicMenuService;

    public PublicMenuController(IPublicMenuService publicMenuService)
    {
        _publicMenuService = publicMenuService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PublicMenuResponseDTO>>> AllProducts()
    {
        var products = await _publicMenuService.GetProductsAsync();
        
        return Ok(products);
    }
    
}