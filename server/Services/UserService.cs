using server.Data;
using server.Dto;
using server.Models;

namespace server.Services;

public class UserService : IUserService
{
  private readonly IUserRepository _userRepository;
  private readonly IMovieRepository _movieRepository;

  public UserService(IUserRepository userRepository, IMovieRepository movieRepository)
  {
    _userRepository = userRepository;
    _movieRepository = movieRepository;
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
        var movie = await _movieRepository.GetMovieByTitleAsync(dto.MovieTitle); //might should convert both to lowercase and strip special chars
        if (movie == null)
        {
          throw new Exception("Movie not found");
        }
        //map dto -> usermovie
    }

    public Task<List<Movie>> GetUserMoviesAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserById(int id)
    {
        User user =  await _userRepository.GetUserById(id);
      if(user == null){
        throw new Exception("User with this id not found");
      }
      else{
          return user;
      }
    }

        public async Task<User> GetUserByName(string name)
    {
      User user = await _userRepository.GetUserByUsernameAsync(name);
      if(user == null){
        throw new Exception("User with this name not found");
      }
      else{
              return user;
      }
    }

    }



