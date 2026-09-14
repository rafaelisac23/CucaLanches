using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.Users.DTOs;

public class UserResponseDTO
{
   public int Id { get; set; }
   public string Name { get; set; }
   public string Email { get; set; }
   public UserRole Role { get; set; }
}