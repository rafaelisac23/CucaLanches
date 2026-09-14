using System.ComponentModel.DataAnnotations;
using CucaLanches.Application.Common;
using CucaLanches.Application.Exceptions;
using CucaLanches.Application.Users.DTOs;
using CucaLanches.Application.Users.Interfaces;
using CucaLanches.Application.Validators;
using CucaLanches.Domain.Entities;
using ValidationException = CucaLanches.Application.Exceptions.ValidationException;

namespace CucaLanches.Application.Users.Services;

public class UserService:IUserService
{
    
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository repository,IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher=passwordHasher;
    }
    
    public async Task<List<UserResponseDTO>> ListAsync()
    {
        var users = await _repository.GetAllAsync();

        var response = users.Select(u => new UserResponseDTO()
        {
            Id = u.Id,
            Email = u.Email,
            Name =  u.Name,
            Role = u.Role
        }).ToList();

        return response;
    }

    public async Task<UserResponseDTO> CreateAsync(UserRequestDTO user)
    {
        
        var erros = UserValidator.IsValid(user);
        
        if (erros.Any()) throw new ValidationException(erros);

        var newUser = new User()
        {
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            PasswordHash = _passwordHasher.Hash(user.Password)
        };
        
        await _repository.CreateAsync(newUser);

        return new UserResponseDTO()
        {
            Name = newUser.Name,
            Email = newUser.Email,
            Role = newUser.Role,
            Id = newUser.Id
        };
    }

    public async Task<bool>  DeleteAsync(int id, int requestingUserId)
    {
        var user = await _repository.GetByIdAsync(id);

        if(user == null) throw new NotFoundException("User not found");
            
        if (user.Id == requestingUserId) throw new InvalidOperationException("You don't delete yourself from the system.");
        
        await _repository.DeleteAsync(user);
        
        return true;
    }
    
    
}