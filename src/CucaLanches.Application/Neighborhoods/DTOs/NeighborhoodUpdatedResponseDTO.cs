namespace CucaLanches.Application.Neighborhoods.DTOs;

public class NeighborhoodUpdatedResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal DeliveryFee {get; set;}
    public bool IsAvaible { get; set; } 
}