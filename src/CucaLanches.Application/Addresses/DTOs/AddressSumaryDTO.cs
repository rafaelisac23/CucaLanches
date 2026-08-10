namespace CucaLanches.Application.Addresses.DTOs;

public class AddressSumaryDTO
{
    public int Id { get; set; }
    public string Cep { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public int HouseNumber { get; set; }
    public string? Description { get; set; }

    public int NeighborhoodId { get; set; }
    public string NeighborhoodName { get; set; } = string.Empty;
    public decimal DeliveryFee { get; set; }
}