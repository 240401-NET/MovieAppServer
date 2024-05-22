using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Dto;

using Microsoft.EntityFrameworkCore;

namespace server.Data;

public class TMDBApiRepository : ITMDBApiRepository
{
  private readonly MovieContext _context;

  public TMDBApiRepository(MovieContext context)
  {
    _context = context;
  }

  public async Task AddMovieToDb(Movie movie) {
    _context.Movies.Add(movie);
    await _context.SaveChangesAsync();
  }
}