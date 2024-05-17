using server.Models;

namespace server.Data;

public class UserMovieRepository : IUserMovieRepository
{
  private readonly MovieContext _context;

  public UserMovieRepository(MovieContext context)
  {
    _context = context;
  }

    public async Task AddUserMovieAsync(UserMovie userMovie)
    {
        _context.UserMovies.Add(userMovie);
        await _context.SaveChangesAsync();
    }
}
