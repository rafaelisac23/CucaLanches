using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.Products.DTOs;

public class ProductRequestDTO
{
    public required string Name { get; set; }
    public ProductType Type { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}