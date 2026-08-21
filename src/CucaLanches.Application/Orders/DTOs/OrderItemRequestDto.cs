namespace CucaLanches.Application.Orders.DTOs;

public class OrderItemRequestDto
{
   public int ProductId { get; set; }
   public int Quantity { get; set; }
   public string? Description { get; set; }
}