using CucaLanches.Application.Exceptions;
using CucaLanches.Application.Products.DTOs;
using CucaLanches.Application.PublicMenu.DTOs;
using CucaLanches.Application.PublicMenu.Interfaces;

namespace CucaLanches.Application.PublicMenu.Services;

public class PublicMenuService:IPublicMenuService
{
    
    private readonly IPublicMenuRepository _repository;

    public PublicMenuService(IPublicMenuRepository  repository)
    {
        _repository = repository;
    }
    
    public async Task<List<PublicMenuResponseDTO>> GetProductsAsync()
    {
        var products = await _repository.GetAllAsync();

        if (!products.Any())
        {
            throw new NotFoundException("Dont have any product");
        }

        var productsResponse = products.GroupBy(p => p.Type)
            .Select(p => new PublicMenuResponseDTO
            {
              Type = p.Key.ToString(),
              Products = p.Select(p=> new ProductResponseDTO
              {
                  Id = p.Id, 
                  Name = p.Name,
                  Active = p.Active,
                  Description = p.Description,
                  Price = p.Price
              }).ToList()
            });
        
        return productsResponse.ToList();
    }
}