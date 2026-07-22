using CucaLanches.Application.Products.DTOs;

namespace CucaLanches.Application.Products.Interfaces;

public interface IProductService
{
    
    Task<List<ProductResponseDTO>> GetAllAsync();
    Task<ProductResponseDTO> GetByIdAsync(int id);
    Task<ProductResponseDTO> CreateAsync(ProductRequestDTO productRequestDto);
    Task<ProductResponseDTO> UpdateAsync(int id, ProductRequestDTO productRequestDto);
    Task<ProductResponseDTO> DesactiveAsync(int id);
    
}