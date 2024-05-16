using server.Models;

namespace server.Data;

public interface IMovieRepository
{
  Task<Movie> GetMovieByTitleAsync(string title);
}