namespace CucaLanches.Application.Addresses.DTOs;

public class AddressRequestPatchDTO
{
    public int? NeighborhoodId { get; set; }
    public string? Cep { get; set; }
    public string? StreetName { get; set; }
    public int? HouseNumber { get; set; }
    public string? Description {get; set;}
}