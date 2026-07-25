using CucaLanches.Application.Products.DTOs;
using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.PublicMenu.DTOs;

public class PublicMenuResponseDTO
{
    public string Type {get; set;} = string.Empty;
    public List<ProductResponseDTO> Products { get; set; } 
}