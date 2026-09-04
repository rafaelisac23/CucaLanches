using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Users.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}