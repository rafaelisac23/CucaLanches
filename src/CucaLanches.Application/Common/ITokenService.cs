using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Common;

public interface ITokenService
{
    string GenerateUserToken(User user);
    string GenerateClientToken(Client client);
}