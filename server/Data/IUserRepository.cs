using server.Models;

namespace server.Data;

public interface IUserRepository
{
  Task<User> GetUserByUsernameAsync(string username);
}