using CucaLanches.Application.Orders.DTOs;
using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.Orders.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto> CreateAsync(CreateOrderRequestDto req);
    Task<List<OrderResponseDto>> GetOrdersByDateAndOrderStatusAsync(DateTime date, OrderStatus status);
    Task<OrderResponseDto> GetOrderByIdAsync(int orderId);
    Task<OrderResponseDto> ChangeStatusAsync(int orderId, OrderStatus status);
}