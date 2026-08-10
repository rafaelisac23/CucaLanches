using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Neighborhoods.Interfaces;

public interface INeighborhoodRepository
{
    Task<List<Neighborhood>> GetAllAsync();
    Task<List<Neighborhood>> GetAllExceptInactiveAsync();
    Task<Neighborhood?> GetByName(string name);
    Task<Neighborhood?> GetById(int id);
    Task AddAync (Neighborhood  neighborhood);
    Task UpdateAync (Neighborhood  neighborhood);
    Task<bool> ExistAsync(int id);
    Task DeleteAsync (Neighborhood  neighborhood);
}