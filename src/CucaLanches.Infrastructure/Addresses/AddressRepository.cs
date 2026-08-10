using CucaLanches.Application.Addresses.Interfaces;
using CucaLanches.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CucaLanches.Infrastructure.Addresses;

public class AddressRepository:IAddressRepository
{
    private readonly AppDbContext _dbContext;

    public AddressRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Address>> GetAll()
    {
        return await _dbContext.Addresses
            .AsNoTracking()
            .Include( x=>x.Neighborhood)
            .ToListAsync();
    }

    public async Task<Address?> GetByIdAsync(int id)
    {
        return await _dbContext.Addresses
            .Include( x=>x.Neighborhood)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Address address)
    {
       _dbContext.Addresses.Add(address);
       await _dbContext.SaveChangesAsync();
    }

    public async Task PatchAsync(Address address)
    {
       _dbContext.Addresses.Update(address);
       await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Address address)
    {
        _dbContext.Addresses.Remove(address);
        await _dbContext.SaveChangesAsync();
    }
}