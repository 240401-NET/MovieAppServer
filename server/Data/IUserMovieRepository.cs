using server.Models;

namespace server.Data;

public interface IUserMovieRepository
{
  Task AddUserMovieAsync(UserMovie userMovie);
  Task<List<Movie>> GetUserMoviesAsync(int id);

}