namespace CucaLanches.Domain.Entities;

public class Client
{
    public int id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Address> Addresses { get; set; } = [];
}