using CucaLanches.Application.Users.Interfaces;
using CucaLanches.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CucaLanches.Infrastructure.Users;

public class UserRepository:IUserRepository
{
    private readonly AppDbContext _dbContext;
    
    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstAsync(u => u.Email == email);
    }
}