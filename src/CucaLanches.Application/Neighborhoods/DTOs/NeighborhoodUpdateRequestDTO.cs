namespace CucaLanches.Application.Neighborhoods.DTOs;

public class NeighborhoodUpdateRequestDTO
{
    public string? Name { get; set; }
    public decimal? DeliveryFee {get; set;}
    public bool? IsAvaible { get; set; } = true;
}