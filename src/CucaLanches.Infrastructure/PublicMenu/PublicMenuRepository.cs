using CucaLanches.Application.PublicMenu.Interfaces;
using CucaLanches.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CucaLanches.Infrastructure.PublicMenu;

public class PublicMenuRepository:IPublicMenuRepository
{
    private readonly AppDbContext _dbContext;

    public PublicMenuRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products.AsNoTracking().Where(p=> p.Active == true).ToListAsync();
    }
    
}