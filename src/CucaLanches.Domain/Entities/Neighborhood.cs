namespace CucaLanches.Domain.Entities;

public class Neighborhood
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal DeliveryFee { get; set; }
    public bool IsAvaliable { get; set; } = true;
}