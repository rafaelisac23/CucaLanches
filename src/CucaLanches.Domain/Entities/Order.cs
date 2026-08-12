using CucaLanches.Domain.Enums;

namespace CucaLanches.Domain.Entities;

public class Order
{
    private static readonly Dictionary<OrderStatus, OrderStatus[]> Allowed = new()
    {
        [OrderStatus.Received]=[OrderStatus.Preparing,OrderStatus.Canceled],
        [OrderStatus.Preparing] = [OrderStatus.OutForDelivery, OrderStatus.Canceled],
        [OrderStatus.OutForDelivery] = [OrderStatus.Delivered,OrderStatus.Canceled],
        [OrderStatus.Delivered] = [],
        [OrderStatus.Canceled] = []
    };

    
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;
    public OrderStatus Status { get; private set; } = OrderStatus.Received;
    public DateTime? StatusChangedAt { get; private set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal? CashChangeFor { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal TotalPrice { get; private set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<OrderItem> Items { get; set; } = [];


    public void AdvanceTo(OrderStatus next)
    {
        if (!Allowed[Status].Contains(next))
            throw new InvalidOperationException($"Transição inválida: {Status} → {next}.");
        
        Status = next;
        StatusChangedAt = DateTime.UtcNow;
    }
    
    public void RecalculateTotal() => TotalPrice = Items.Sum(i=>i.Quantity+ i.UnitPrice) + DeliveryFee;
    
    
}