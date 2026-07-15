using CucaLanches.Domain.Enums;

namespace CucaLanches.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ProductType Type { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}