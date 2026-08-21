using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Orders.Interfaces;

public interface IOrderRepository
{
    Task CreateAsync(Order order);
    Task<Order?> GetByIdAsync(int orderId);
}