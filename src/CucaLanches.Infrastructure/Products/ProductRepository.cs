using CucaLanches.Application.Products.Interfaces;
using CucaLanches.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CucaLanches.Infrastructure.Products;

public class ProductRepository:IProductRepository
{
    
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<List<Product>> GetAll()
    {
    return await _dbContext.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
       return await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p=> p.Id == id);
    }
    
    public async Task AddAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
       await UpdateOrDisactive(product);
    }

    public async Task DeleteAsync(Product product)
    {
       await UpdateOrDisactive(product);
    }
    
    public async Task UpdateOrDisactive(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }
    
}