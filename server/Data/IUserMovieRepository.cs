using server.Models;
using server.Dto;
namespace server.Data;

public interface IUserMovieRepository
{
  Task AddUserMovieAsync(UserMovie userMovie);

  Task<List<Movie>> ListFavoriteMovies(string id);
  Task RemoveMovieFromUser(FavoritedMovieDto dto);

}