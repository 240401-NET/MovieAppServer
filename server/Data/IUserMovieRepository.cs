using server.Models;

namespace server.Data;

public interface IUserMovieRepository
{
  Task AddUserMovieAsync(UserMovie userMovie);
  Task RemoveUserMovieAsync(string userId, int movieId);
  Task<List<Movie>> GetUserFavorited(string userId);
}