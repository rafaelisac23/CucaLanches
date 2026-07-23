using System.Text.Json;
using CucaLanches.Application.Exceptions;
using CucaLanches.Application.Products.DTOs;
using CucaLanches.Application.Products.Interfaces;
using CucaLanches.Application.Validators;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Products.Services;

public class ProductService:IProductService
{
    
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductResponseDTO>> GetAllAsync()
    {
        var result = await _repository.GetAll();

        if (!result.Any())
        {
            throw new NotFoundException("Product not found");
        }
        
        var response = result.Select(p => new ProductResponseDTO
        {
            Id =  p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Active = p.Active,
            Type = p.Type.ToString()
        });
        
        return response.ToList();
    }

    public async Task<ProductResponseDTO> GetByIdAsync(int id)
    {
        
        var result = await _repository.GetById(id);

        if (result == null)
        {
            throw new NotFoundException("Product not found");
        }

        var productDto = new ProductResponseDTO
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            Type = result.Type.ToString(),
            Price = result.Price,
            Active = result.Active,
        };
        
        return productDto;

    }
    
    public async Task<ProductResponseDTO> CreateAsync(ProductRequestDTO request)
    {
     var errors = ProductValidator.IsValid(request);

     if (errors.Any())
     {
         throw new ValidationException(errors);
     }

     var product = new Product()
     {
         Name = request.Name!,
         Type = request.Type.Value,
         Description = request.Description,
         Price = request.Price.Value,
         Active = true
     };
     
     await _repository.AddAsync(product);

     var response = new ProductResponseDTO
     {
         Id = product.Id,
         Name = product.Name,
         Description = product.Description,
         Price = product.Price,
         Active = true,
         Type = product.Type.ToString()
     };
     
     return response;
     
    }

    public async Task<ProductResponseDTO> UpdateAsync(int id, ProductRequestDTO productRequestDto)
    {
       var errors =  ProductValidator.IsValid(productRequestDto);

       if (errors.Any())
       {
           throw new ValidationException(errors);
       }
       
       var product = await _repository.GetById(id);

        if (product == null)
        {
            throw new NotFoundException("Product not found");
        }
        
        product.Name = productRequestDto.Name;
        product.Description = productRequestDto.Description;
        product.Price = productRequestDto.Price.Value;
        product.Type = productRequestDto.Type.Value;

        await _repository.UpdateAsync(product);

        var response = new ProductResponseDTO
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Id = product.Id,
            Active = product.Active,
            Type = product.Type.ToString()
        };
        
        return response;
    }

    public async Task<ProductResponseDTO> DesactiveAsync(int id)
    {
        var product = await _repository.GetById(id);

        if (product == null)
        {
            throw new NotFoundException("Product not found");
        }
        
        product.Active = false;
        
        await _repository.DeleteAsync(product);
        
        var response = new ProductResponseDTO
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Id = product.Id,
            Active = product.Active,
            Type = product.Type.ToString()
        };

        return response;


    }
}