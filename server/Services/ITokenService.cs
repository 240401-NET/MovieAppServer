using server.Models;

namespace server.Services;

public interface ITokenService
{
  string CreateToken(User user);
}