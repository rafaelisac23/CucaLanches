using CucaLanches.Application.Neighborhoods.Interfaces;
using CucaLanches.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CucaLanches.Infrastructure.Neighborhoods;

public class NeighborhoodRepository:INeighborhoodRepository
{
    
    private readonly AppDbContext _dbContext;

    public NeighborhoodRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<List<Neighborhood>> GetAllAsync()
    {
        return await _dbContext.Neighborhoods.AsNoTracking().ToListAsync();
    }

    public async Task<List<Neighborhood>> GetAllExceptInactiveAsync()
    {
        return await _dbContext.Neighborhoods.Where(n => n.IsAvaliable == true).ToListAsync();
    }

    public async Task<Neighborhood?> GetByName(string name)
    {
        return await _dbContext.Neighborhoods.FirstOrDefaultAsync(n =>  n.Name == name);
    }

    public async Task<Neighborhood?> GetById(int id)
    {
        return await _dbContext.Neighborhoods.FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task AddAync(Neighborhood neighborhood)
    {
        _dbContext.Neighborhoods.Add(neighborhood);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Neighborhood neighborhood)
    {
        await UpdateOrDisactive(neighborhood);
    }

    public async Task UpdateAync(Neighborhood neighborhood)
    {
        await UpdateOrDisactive(neighborhood);
    }

    public async Task<bool> ExistAsync(int id)
    {
        return await _dbContext.Neighborhoods.AnyAsync(n => n.Id == id);
    }

    public async Task UpdateOrDisactive(Neighborhood neighborhood)
    {
        _dbContext.Neighborhoods.Update(neighborhood);
        await _dbContext.SaveChangesAsync();
    }
}