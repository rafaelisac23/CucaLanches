using CucaLanches.Application.Auth.DTOs;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Auth.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDTO> LoginUserAsync(UserLoginRequestDTO request);
    Task<AuthResponseDTO> LoginClientAsync(ClientLoginRequestDTO user);
}