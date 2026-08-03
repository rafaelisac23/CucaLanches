using CucaLanches.Application.Clients.DTOs;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Clients.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetByPhoneAsync(string phone);
    Task<Client?> GetByIdAsync(int id);
    Task AddAsync(Client client);
    Task PatchAsync(Client client);
}