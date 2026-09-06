namespace CucaLanches.Domain.Entities;

public sealed class Client
{
    public int id { get; private set; }
    public required string Name { get; private set; }
    public required string Phone { get; private set; }
    public string? Email { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<Address> Addresses { get; set; } = [];

}
