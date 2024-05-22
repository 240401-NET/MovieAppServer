using server.Dto;
using server.Models;

namespace server.Services;
public interface IUserService
{ 
  Task<List<Movie>> GetUserMoviesAsync(string userId);
  Task AddMovieToUser(FavoritedMovieDto dto);
  Task RemoveMovieFromUser(FavoritedMovieDto dto);

}