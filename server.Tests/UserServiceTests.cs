using server.Dto;
using server.Data;
using server.Models;
using server.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Moq;
namespace server.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _moqUserRepo;
    private readonly Mock<IMovieRepository> _moqMovieRepo;
    private readonly Mock<IUserMovieRepository> _moqUserMovieRepo;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _moqUserRepo = new Mock<IUserRepository>();
        _moqMovieRepo = new Mock<IMovieRepository>();
        _moqUserMovieRepo = new Mock<IUserMovieRepository>();
        var httpClient = new HttpClient();
        var mockConfig = new Mock<IConfiguration>();
        _userService = new UserService(_moqUserRepo.Object, _moqMovieRepo.Object, _moqUserMovieRepo.Object, new TMDBService(httpClient, mockConfig.Object));
    }

    static async Task SomeAsyncMethod()
    {
        // Simulate an asynchronous operation
        await Task.Delay(1000); // Delay for 1 second
    }

    [Fact]
    public async void UserService_GetUserMoviesAsync_ReturnsListOfFavoriteMovies()
    {
        // Arrange

        // Set Up List of Movies
        Movie movieOne = new Movie
        {
            MovieId = 1,
            Title = "The Shawshank Redemption",
            ReleaseDate = DateTime.Now,
            PurchasedTickets = false,
            MovieLanguage = "testLanguage",
            PosterPath = "n/a"
        };
        List<Movie> movieList = new List<Movie> { movieOne };

        // Setup User
        User oneUser = new User()
        {
            Id = "1",
            UserName = "TestUser",
            Email = "Test@email.com",
            Age = 18,
            Movie = movieOne
        };
        // Setp Up Moq
        _moqUserRepo.Setup(r => r.GetUserByUsernameAsync(oneUser.UserName)).ReturnsAsync(oneUser);
        _moqUserMovieRepo.Setup(r => r.ListFavoriteMovies(oneUser.Id)).ReturnsAsync(movieList);

        // Act
        Task<List<Movie>> result = _userService.GetUserMoviesAsync(oneUser.UserName);
        List<Movie> resultMovieList = await result;

        // Assert
        Assert.NotNull(resultMovieList);
        Assert.Equal(resultMovieList.Count, movieList.Count);
    }

    [Fact]
    public void UserSerVice_AddMovieToUser_ReturnsTask()
    {
        // Arrange

        // Set up favMovie DTO
        FavoritedMovieDto favMovieDto = new FavoritedMovieDto()
        {
            Username = "TestUser",
            MovieTitle = "The Shawshank Redemption",
            MovieId = 1,
            PosterPath = "n/a",
            Description = "The blue seas of Zihuatanejo"
        };
        // Setup User
        User oneUser = new User()
        {
            Id = "1",
            UserName = "TestUser",
            Email = "Test@email.com"
        };
        Movie movieOne = new Movie
        {
            MovieId = 1,
            Title = "The Shawshank Redemption",
            ReleaseDate = DateTime.Now,
            PurchasedTickets = false,
            MovieLanguage = "testLanguage",
            PosterPath = "n/a"
        };
        // Set up User Movie
        UserMovie userMovie = new UserMovie()
        {
            UserId = oneUser.Id,
            User = oneUser,
            MovieId = favMovieDto.MovieId,
            Movie = movieOne,
            Description = favMovieDto.Description,
            PosterPath = favMovieDto.PosterPath
        };
        // Set up Repo Moqs
        _moqUserRepo.Setup(r => r.GetUserByUsernameAsync(oneUser.Id)).ReturnsAsync(oneUser);
        _moqMovieRepo.Setup(r => r.GetMovieByIdAsync(favMovieDto.MovieId)).ReturnsAsync(movieOne);
        _moqUserMovieRepo.Setup(r => r.AddUserMovieAsync(userMovie));
        Task moqTask = SomeAsyncMethod();

        // Act

        var result = _userService.AddMovieToUser(favMovieDto);

        // Assert

        Assert.NotNull(result);
    }

    [Fact]
    public void UserService_RemoveMovieFromUser_ReturnsTask()
    {
        // Arrange

        // Create DTO
        FavoritedMovieDto movieDtoDelete = new FavoritedMovieDto()
        {
            Username = "TestUser",
            MovieTitle = "The Shawshank Redemption",
            MovieId = 1,
            PosterPath = "n/a",
            Description = "The blue seas of Zihuatanejo"
        };
        // Setup User
        User oneUser = new User()
        {
            Id = "1",
            UserName = "TestUser",
            Email = "Test@email.com"
        };

        // Setup the Repo Moqs
        _moqUserRepo.Setup(r => r.GetUserByUsernameAsync(movieDtoDelete.Username)).ReturnsAsync(oneUser);
        _moqUserMovieRepo.Setup(r => r.RemoveMovieFromUser(movieDtoDelete));

        // Act
        var result = _userService.RemoveMovieFromUser(movieDtoDelete);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async void UserService_GetUserMoviesAsync_ThrowsErrorWhenUserIsNull()
    {
        // Arrange

        // Set Up List of Movies
        Movie movieOne = new Movie
        {
            MovieId = 1,
            Title = "The Shawshank Redemption",
            ReleaseDate = DateTime.Now,
            PurchasedTickets = false,
            MovieLanguage = "testLanguage",
            PosterPath = "n/a"
        };
        List<Movie> movieList = new List<Movie> { movieOne };

        // Setup User
        User oneUser = new User()
        {
            Id = "1",
            UserName = "TestUser",
            Email = "Test@email.com",
            Age = 18,
            Movie = movieOne
        };
        // Setp Up Moq
        _moqUserRepo.Setup(r => r.GetUserByUsernameAsync(oneUser.UserName)).ReturnsAsync((User)null);
        _moqUserMovieRepo.Setup(r => r.ListFavoriteMovies(oneUser.Id)).ReturnsAsync(movieList);

        // Act
        await Assert.ThrowsAnyAsync<Exception>(() => _userService.GetUserMoviesAsync(oneUser.UserName));
    }

    [Fact]
    public async void UserService_RemoveMovieFromUser_ThrowsAnException()
    {
        // Arrange

        // Create DTO
        FavoritedMovieDto movieDtoDelete = new FavoritedMovieDto()
        {
            Username = "TestUser",
            MovieTitle = "The Shawshank Redemption",
            MovieId = 1,
            PosterPath = "n/a",
            Description = "The blue seas of Zihuatanejo"
        };
        // Setup User
        User oneUser = new User()
        {
            Id = "1",
            UserName = "TestUser",
            Email = "Test@email.com"
        };

        // Setup the Repo Moqs
        _moqUserRepo.Setup(r => r.GetUserByUsernameAsync(movieDtoDelete.Username)).ReturnsAsync((User)null);
        _moqUserMovieRepo.Setup(r => r.RemoveMovieFromUser(movieDtoDelete));

        // Act and Assert
        await Assert.ThrowsAnyAsync<Exception>(() => _userService.RemoveMovieFromUser(movieDtoDelete));
        ;

    }

     [Fact]
    public async void UserSerVice_AddMovieToUser_ThrowsExceptionToNullUser()
    {
        // Arrange

        // Set up favMovie DTO
        FavoritedMovieDto favMovieDto = new FavoritedMovieDto()
        {
            Username = "TestUser",
            MovieTitle = "The Shawshank Redemption",
            MovieId = 1,
            PosterPath = "n/a",
            Description = "The blue seas of Zihuatanejo"
        };
        // Setup User
        User oneUser = new User()
        {
            Id = "1",
            UserName = "TestUser",
            Email = "Test@email.com"
        };
        Movie movieOne = new Movie
        {
            MovieId = 1,
            Title = "The Shawshank Redemption",
            ReleaseDate = DateTime.Now,
            PurchasedTickets = false,
            MovieLanguage = "testLanguage",
            PosterPath = "n/a"
        };
        // Set up User Movie
        UserMovie userMovie = new UserMovie()
        {
            UserId = oneUser.Id,
            User = oneUser,
            MovieId = favMovieDto.MovieId,
            Movie = movieOne,
            Description = favMovieDto.Description,
            PosterPath = favMovieDto.PosterPath
        };
        // Set up Repo Moqs
        _moqUserRepo.Setup(r => r.GetUserByUsernameAsync(oneUser.Id)).ReturnsAsync((User)null);
        _moqMovieRepo.Setup(r => r.GetMovieByIdAsync(favMovieDto.MovieId)).ReturnsAsync(movieOne);
        _moqUserMovieRepo.Setup(r => r.AddUserMovieAsync(userMovie));
        var httpClient = new HttpClient();
        var mockConfig = new Mock<IConfiguration>();
        UserService userService = new UserService(_moqUserRepo.Object, _moqMovieRepo.Object, _moqUserMovieRepo.Object, new TMDBService(httpClient, mockConfig.Object));

        // Act and Assert
        await Assert.ThrowsAnyAsync<Exception>(() => userService.AddMovieToUser(favMovieDto));
    }
}