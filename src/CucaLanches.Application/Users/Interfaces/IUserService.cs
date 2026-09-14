using CucaLanches.Application.Users.DTOs;

namespace CucaLanches.Application.Users.Interfaces;

public interface IUserService
{
    Task<List<UserResponseDTO>> ListAsync();
    Task<UserResponseDTO> CreateAsync(UserRequestDTO user);
    Task<bool> DeleteAsync(int id, int requestingUserId);
}