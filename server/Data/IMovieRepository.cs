using server.Models;

namespace server.Data;

public interface IMovieRepository
{
  Task<Movie> GetMovieByTitleAsync(string title);
  Task<List<Movie>> GetMoviesByGenreAsync(string genre);
  Task<List<Movie>> GetMovieByLanguageAsync(string language);
  Task AddMovieAsync(Movie movie);
  Task<Movie> GetMovieByIdAsync(int movieId);
}