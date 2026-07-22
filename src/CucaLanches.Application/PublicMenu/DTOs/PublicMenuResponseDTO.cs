using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.PublicMenu.DTOs;

public class PublicMenuResponseDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Type { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}