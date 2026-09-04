using System.ComponentModel.DataAnnotations;
using CucaLanches.Application.Auth.DTOs;
using CucaLanches.Application.Auth.Interfaces;
using CucaLanches.Application.Clients.Interfaces;
using CucaLanches.Application.Common;
using CucaLanches.Application.Exceptions;
using CucaLanches.Application.Users.Interfaces;
using CucaLanches.Application.Validators;
using ValidationException = CucaLanches.Application.Exceptions.ValidationException;

namespace CucaLanches.Application.Auth.Services;

public class AuthService:IAuthService
{
    
    private readonly IUserRepository _userRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IClientRepository clientRepository,ITokenService tokenService,IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _clientRepository = clientRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<AuthResponseDTO> LoginUserAsync(UserLoginRequestDTO request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new ValidationException(
                [new ValidationError()
                {
                    Field = "Credentials",
                    Message = "Invalid credentials"
                }], 401);
        }
        
        var token = _tokenService.GenerateUserToken(user);
        return new AuthResponseDTO() {AccessToken =  token,ExpiresAt = DateTime.UtcNow.AddHours(2)};
    }

    public async Task<AuthResponseDTO> LoginClientAsync(ClientLoginRequestDTO user)
    {
        var phone = PhoneNormalizer.Normalize(user.Phone);
        var client = await  _clientRepository.GetByPhoneAsync(phone);

        if (client is null)
        {
            throw new NotFoundException("Don't found a client with its phone number");
        }
        
        var token = _tokenService.GenerateClientToken(client);
        
        return new AuthResponseDTO() {AccessToken =  token,ExpiresAt = DateTime.UtcNow.AddHours(2)};
    }
}