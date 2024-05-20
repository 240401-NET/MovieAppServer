using Microsoft.EntityFrameworkCore;
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
    var movie = await _context.Movies.FindAsync(userMovie.MovieId);
    if (movie != null)
    {
      _context.UserMovies.Add(userMovie);
      await _context.SaveChangesAsync();
    } else 
    {
      throw new Exception("Movie not favorited");
    }
  }

  public async Task<List<Movie>> GetUserFavorited(string userId)
  {
    return await _context.UserMovies.Where(um => um.UserId == userId)
    .Select(um => um.Movie)
    .ToListAsync();
  }

    public async Task RemoveUserMovieAsync(string userId, int movieId)
    {
        var userMovie = await _context.UserMovies
        .FirstOrDefaultAsync(um => um.UserId == userId && um.MovieId == movieId);
        
        if (userMovie != null) {
          _context.UserMovies.Remove(userMovie);
          await _context.SaveChangesAsync();
        }
    }
}
