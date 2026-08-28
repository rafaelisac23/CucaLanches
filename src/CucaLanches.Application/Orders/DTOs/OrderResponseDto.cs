using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.Orders.DTOs;

public class OrderResponseDto
{
   public int Id {get; set;}
   public int OrderNumber { get; set; }
   public string? Status  { get; set; }
   public PaymentMethod PaymentMethod { get; set; }
   public decimal? CashChangeFor { get; set; }
   public decimal DeliveryFee { get; set; }
   public decimal TotalPrice { get; set; }
   public DateTime CreatedAt { get; set; }
   public string? ClientName { get; set; }
   public string? ClientPhone { get; set; }
   public string? AddressSummary { get; set; }
   public List<OrderItemResponseDto> Items { get; set; } = null!;
}