using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CucaLanches.Application.Common;
using CucaLanches.Application.Exceptions;
using CucaLanches.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CucaLanches.Infrastructure;

public class TokenService:ITokenService
{
    
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
            _configuration = configuration;    
    }
    
    public string GenerateClientToken(Client client)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, client.id.ToString()),
            new(ClaimTypes.Name, client.Name),
            new("phone", client.Phone),
            new("type", "client")
        };
          
          return GenerateToken(claims);
    }

    public string GenerateUserToken(User user)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("type", "user")
        };
          
        return GenerateToken(claims);
    }
    
    private string GenerateToken(List<Claim> claims)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new NotFoundException("Doesn't exist a security key");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
        var jwtExpiration = _configuration["Jwt:ExpiresInMinutes"] ?? throw new NotFoundException("Doesn't exist a expires time key"); 
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtExpiration));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: cred
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}