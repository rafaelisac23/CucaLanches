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
            throw new Exception("Dont have any products");
        }

        var productsResponse = products.Select(p => new PublicMenuResponseDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description =  p.Description,
            Type = p.Type.ToString(),
            Price = p.Price
        });
        
        return productsResponse.ToList();
    }
}