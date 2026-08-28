using CucaLanches.Application.Exceptions;
using CucaLanches.Application.Orders.DTOs;
using CucaLanches.Application.Orders.Interfaces;
using CucaLanches.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CucaLanches.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class OrderController:ControllerBase
{
    
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> CreateAsync(CreateOrderRequestDto req)
    {
        var result = await _orderService.CreateAsync(req);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseDto>>> GetAsync([FromQuery]DateTime? date,[FromQuery] OrderStatus? status)
    {
        if (!ModelState.IsValid)
        {
            throw new OrderRuleException("The Model is invalid");
        }
        
        if (date is null)
        {
            throw new OrderRuleException("Date is invalid");
        }
        
        if (status is null)
        {
            throw new OrderRuleException("Status is invalid");
        }
        
        var result = await _orderService.GetOrdersByDateAndOrderStatusAsync(date.Value.Date, status.Value);
        
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetByIdAsync(int id)
    {
        var result = await _orderService.GetOrderByIdAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<OrderResponseDto>> ChangeStatus([FromRoute]int id,[FromQuery]OrderStatus status)
    {
        var result = await _orderService.ChangeStatusAsync(id, status);
        return Ok(result);
    }
    
    
    
}