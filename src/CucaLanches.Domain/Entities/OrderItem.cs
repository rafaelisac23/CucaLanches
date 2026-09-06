namespace CucaLanches.Domain.Entities;

public sealed class OrderItem
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string? Description { get; private set; }
    
}
