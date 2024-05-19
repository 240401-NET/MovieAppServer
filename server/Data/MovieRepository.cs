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
    int genreId = MapGenreToId(genre);
    return await _context.Movies.Where(m => m.Genre == genre).ToListAsync();
  }

  private int MapGenreToId(string genre)
  {
    //if we are hardcoding genre id's, map that here. Probably should be in service
    throw new NotImplementedException();
  }


}