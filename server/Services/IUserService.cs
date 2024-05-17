using server.Dto;
using server.Models;

namespace server.Services;
public interface IUserService
{
  Task AddMovieToUser(FavoritedMovieDto dto);
  //Task RemoveMovieFromUser(FavoritedMovieDto dto);
  Task<List<Movie>> GetUserMoviesAsync(string userId);
  Task<User> GetUserById(int id);
  Task<User> GetUserByName(string name);

}