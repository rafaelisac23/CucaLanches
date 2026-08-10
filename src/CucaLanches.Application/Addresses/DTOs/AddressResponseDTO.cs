using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Addresses.DTOs;

public class AddressResponseDTO
{
    public int Id { get; set; }

    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;

    public int NeighborhoodId { get; set; }
    public string NeighborhoodName { get; set; } = string.Empty;
    public decimal DeliveryFee { get; set; }

    public string Cep { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public int HouseNumber { get; set; }
    public string? Description { get; set; }
}