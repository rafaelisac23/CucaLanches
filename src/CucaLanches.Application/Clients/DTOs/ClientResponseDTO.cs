using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Clients.DTOs;

public class ClientResponseDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? Email { get; set; }
    public List<Address> Addresses { get; set; } = [];
}