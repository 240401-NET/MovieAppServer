using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Services;

namespace server.Data;

public class MovieRepository : IMovieRepository
{
  private readonly MovieContext _context;

  public MovieRepository(MovieContext context)
  {
    _context = context;
  }
  //All probably going to be api calls
  public async Task<Movie> GetMovieByTitleAsync(string title)
  {
    return await _context.Movies.FirstOrDefaultAsync(m => m.Title == title);
  }
  public async Task<List<Movie>> GetMovieByLanguageAsync(string language)
  {
    return await _context.Movies.Where(m => m.MovieLanguage == language).ToListAsync();
  }
  public async Task<List<Movie>> GetMoviesByGenreAsync(string genre)
  {
    return await _context.Movies.Where(m => m.Genre == genre).ToListAsync();
  }

 


}