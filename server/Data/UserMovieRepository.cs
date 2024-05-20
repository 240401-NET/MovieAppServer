using server.Models;

namespace server.Data;

public class UserMovieRepository : IUserMovieRepository
{
  private readonly MovieContext _context;

  public UserMovieRepository(MovieContext context)
  {
    _context = context;
  }

    public async Task AddUserMovieAsync(UserMovie userMovie)
    {
        _context.UserMovies.Add(userMovie);
        await _context.SaveChangesAsync();
    }

    //GET ALL MOVIES FOR A USER
    public async Task<List<Movie>> GetUserMoviesAsync(int id){
        var favoriteMovies = await (from userMovie in _context.UserMovies
                                join movie in _context.Movies
                                on userMovie.MovieId equals movie.Id
                                where userMovie.UserId == userId
                                select movie).ToListAsync();

        return favoriteMovies;
    }

}
