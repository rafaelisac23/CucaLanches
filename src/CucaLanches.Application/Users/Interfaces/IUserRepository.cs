using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Users.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task CreateAsync(User user);
    Task DeleteAsync(User user);
}