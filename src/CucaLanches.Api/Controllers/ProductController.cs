
using CucaLanches.Application.Products.DTOs;
using CucaLanches.Application.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CucaLanches.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController:ControllerBase
{
  private readonly IProductService _productService;

  public ProductController(IProductService productService)
  {
    _productService = productService;
  }


  [HttpGet]
  public async Task<ActionResult<List<ProductResponseDTO>>> GetAllAsync()
  {
    var response = await _productService.GetAllAsync();
    
    return Ok(response);
  }
  
  [HttpGet("{id}")]
  public async Task<ActionResult<ProductResponseDTO>> GetByIdAsync(int id)
  {
    var response = await _productService.GetByIdAsync(id);
    
    return Ok(response);
  }
  
  [HttpPost]
  public async Task<ActionResult<ProductResponseDTO>> Create(ProductRequestDTO product)
  {
    var newproduct = await  _productService.CreateAsync(product);
    
    return Ok(newproduct);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<ProductResponseDTO>> UpdateAsync(int id, [FromBody]ProductRequestDTO product)
  {
    var response = await _productService.UpdateAsync(id, product);
    
      return Ok(response);
  }

  [HttpDelete("{id}")]

  public async Task<ActionResult<ProductResponseDTO>> DeleteAsync(int id)
  {
    var response = await _productService.DesactiveAsync(id);
    
    return Ok(response);
  }
    
}