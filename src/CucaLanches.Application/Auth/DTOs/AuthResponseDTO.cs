namespace CucaLanches.Application.Auth.DTOs;

public class AuthResponseDTO
{
    public required string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}