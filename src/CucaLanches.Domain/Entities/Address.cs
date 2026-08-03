namespace CucaLanches.Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public int NeighborhoodId { get; set; }
    public Neighborhood Neighborhood { get; set; } = null!;
    public required string Cep { get; set; }
    public required string StreetName { get; set; }
    public int HouseNumber { get; set; }
    public string? Description {get; set;}
}