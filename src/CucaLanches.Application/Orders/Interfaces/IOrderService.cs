using CucaLanches.Application.Orders.DTOs;

namespace CucaLanches.Application.Orders.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto> CreateAsync(CreateOrderRequestDto req);
}