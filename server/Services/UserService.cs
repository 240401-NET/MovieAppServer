using server.Data;
using server.Dto;
using server.Models;

namespace server.Services;

public class UserService : IUserService
{
  private readonly IUserRepository _userRepository;
  private readonly IMovieRepository _movieRepository;
  private readonly IUserMovieRepository _userMovieRepository;

  public UserService(IUserRepository userRepository, IMovieRepository movieRepository, IUserMovieRepository userMovieRepository)
  {
    _userRepository = userRepository;
    _movieRepository = movieRepository;
    _userMovieRepository = userMovieRepository;
  }

  public async Task AddMovieToUser(FavoritedMovieDto dto) 
  {
    //get user from username & movie from title -> map to usermovie
    //repository methods for get username
    var user = await _userRepository.GetUserByUsernameAsync(dto.Username);
    if (user == null)
    {
      throw new Exception("User not found");
    }
    // var movie = await _movieRepository.GetMovieByTitleAsync(dto.MovieTitle); //might should convert both to lowercase and strip special chars
    // if (movie == null)
    // {
    //   throw new Exception("Movie not found");
    // }
    //map dto -> usermovie
    var userMovie = new UserMovie
    {
      UserId = user.Id,
      MovieId = dto.MovieId
    };

    await _userMovieRepository.AddUserMovieAsync(userMovie);
    }


  //Returns the list of favorite movies of the selected user
  public async Task<List<Movie>> GetUserMoviesAsync(string id)
  {
    User selectedUser = await _userRepository.GetUserByIdAsync(id);
    if(selectedUser == null){
      throw new Exception("User not found");
    }
    else{
       List<Movie> userFavorites =  await _userMovieRepository.ListFavoriteMovies(id);
       return userFavorites;
    }

  }

  //Remove selected movie from the User

  public async Task RemoveMovieFromUser(FavoritedMovieDto dto)
  {
    User selectedUser = await _userRepository.GetUserByUsernameAsync(dto.Username);
    //Movie selectedMovie = await _movieRepository.GetMovieByTitle(dto.MovieTitle);  REMOVE COMMENT AFTER IMPLEMENTING GETBYTITLE

      if(selectedUser == null){
        throw new Exception("Wrong data. Double check username/movie title");
      }
      else{
       await _userMovieRepository.RemoveMovieFromUser(dto);
      }
  }
}