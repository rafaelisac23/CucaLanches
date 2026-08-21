using CucaLanches.Application.Orders.DTOs;
using CucaLanches.Application.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CucaLanches.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class OrderCOntroller:ControllerBase
{
    
    private readonly IOrderService _orderService;

    public OrderCOntroller(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> CreateAsync(CreateOrderRequestDto req)
    {
        var result = await _orderService.CreateAsync(req);
        return Ok(result);
    }
    
}