using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Dto;

using Microsoft.EntityFrameworkCore;

namespace server.Data;

public class UserMovieRepository : IUserMovieRepository
{
  private readonly MovieContext _context;

  public UserMovieRepository(MovieContext context)
  {
    _context = context;
  }

 //ADD NEW FAVORITE MOVIE

    public async Task AddUserMovieAsync(UserMovie userMovie)
    {
        _context.UserMovie.Add(userMovie);
        await _context.SaveChangesAsync();
    }

    //SHOW ALL  FAVORITE MOVIES

    public async Task<List<Movie>> ListFavoriteMovies(string id){
      var movies = await _context.UserMovie.Where(rec => rec.User.Id == id)                               
                                          .Select(rec => rec.Movie)
                                          .ToListAsync();
      return movies;
    }

    //REMOVE A MOVIE FROM FAVORITES
    public async Task RemoveMovieFromUser(FavoritedMovieDto dto){

       // Find the UserMovie entry that corresponds to the user and movie
            var userMovie = await _context.UserMovie.FirstOrDefaultAsync(rec => rec.User.UserName == dto.Username && rec.Movie.Title == dto.MovieTitle);
            if (userMovie != null)
            {
                // Remove the UserMovie entry
                _context.UserMovie.Remove(userMovie);
                await _context.SaveChangesAsync();
            }
}

}