using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Products.Interfaces;

public interface IProductRepository
{
   Task<List<Product>> GetAll();
   Task<Product> GetById(int id);
   Task AddAsync(Product product);
   Task UpdateAsync(Product product);
   Task DeleteAsync(Product product);
  
}