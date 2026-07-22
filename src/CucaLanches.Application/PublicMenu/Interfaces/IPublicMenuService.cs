using CucaLanches.Application.PublicMenu.DTOs;

namespace CucaLanches.Application.PublicMenu.Interfaces;

public interface IPublicMenuService
{
    Task <List<PublicMenuResponseDTO>> GetProductsAsync();
}