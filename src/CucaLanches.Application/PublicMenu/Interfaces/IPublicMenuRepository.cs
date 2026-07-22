using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.PublicMenu.Interfaces;

public interface IPublicMenuRepository
{
    Task <List<Product>> GetAllAsync();

}