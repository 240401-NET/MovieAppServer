using server.Models;
using server.Dto;
namespace server.Data;

public interface IUserMovieRepository
{
  Task AddUserMovieAsync(UserMovie userMovie);
  Task RemoveMovieFromUser(FavoritedMovieDto dto);
  Task<List<Movie>> ListFavoriteMovies(string userId);
}