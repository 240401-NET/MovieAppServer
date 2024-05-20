using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data;

public class UserRepository : IUserRepository
{
  private readonly MovieContext _context;

  public UserRepository(MovieContext context)
  {
    _context = context;
  }

  public async Task<User> GetUserByUsernameAsync(string username)
  {
    return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
  }




}