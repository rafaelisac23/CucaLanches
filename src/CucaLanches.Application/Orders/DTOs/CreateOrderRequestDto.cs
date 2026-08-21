using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.Orders.DTOs;

public class CreateOrderRequestDto
{
    public int ClientId { get; set; }
    public int AddressId { get;set; } 
    public PaymentMethod PaymentMethod { get;set; }
    public decimal? CashChangeFor { get; set; }
    public List<OrderItemRequestDto> Items { get; set; } = [];
}