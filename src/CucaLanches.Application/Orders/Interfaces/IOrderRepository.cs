using CucaLanches.Domain.Entities;
using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.Orders.Interfaces;

public interface IOrderRepository
{
    Task CreateAsync(Order order);
    Task<Order?> GetByIdAsync(int orderId);
    Task<List<Order>> GetOrderByDateAndOrderStatus(DateTime date, OrderStatus status);
    Task ChangeStatusAsync(Order order);
}