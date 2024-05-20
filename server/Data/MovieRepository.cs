using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data;

public class MovieRepository : IMovieRepository
{
  private readonly MovieContext _context;

  public MovieRepository(MovieContext context)
  {
    _context = context;
  }
    public async Task<Movie> GetMovieByTitleAsync(string title)
    {
        return await _context.Movies.FirstOrDefaultAsync(m => m.Title == title);
    }


}