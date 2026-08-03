using CucaLanches.Application.Clients.DTOs;
using CucaLanches.Application.Clients.Interfaces;
using CucaLanches.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CucaLanches.Infrastructure.Clients;

public class ClientRepository:IClientRepository
{
    
    private readonly AppDbContext _dbContext;

    public ClientRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Client?> GetByPhoneAsync(string phone)
    {
        return await _dbContext.Clients
            .Include(a => a.Addresses)
            .ThenInclude(n => n.Neighborhood)
            .FirstOrDefaultAsync(c => c.Phone == phone);

    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _dbContext.Clients
            .Include(a => a.Addresses)
            .ThenInclude(n => n.Neighborhood)
            .FirstOrDefaultAsync(c => c.id == id);
    }

    public async Task AddAsync(Client client)
    {
        _dbContext.Clients.Add(client);
        await _dbContext.SaveChangesAsync();
    }

    public async Task PatchAsync(Client client)
    {
        _dbContext.Clients.Update(client);
        await _dbContext.SaveChangesAsync();
    }
    
}