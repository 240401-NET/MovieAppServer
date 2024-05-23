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

    [Fact]
    public async void UserService_GetUserMoviesAsync_ReturnsListOfFavoriteMovies()
    {
        // Arrange
        
        // Setup User
        User oneUser = new User()
        {
            Id = "1",
            UserName = "TestUser",
            Email = "Test@email.com"
        };
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
        List<Movie> movieList = new List<Movie> {movieOne};
        // Setp Up Moq
        _moqUserRepo.Setup(r => r.GetUserByIdAsync(oneUser.Id)).ReturnsAsync(oneUser);
        _moqUserMovieRepo.Setup(r => r.ListFavoriteMovies(oneUser.Id)).ReturnsAsync(movieList);

        // Act
        Task<List<Movie>> result = _userService.GetUserMoviesAsync(oneUser.Id);
        List<Movie> resultMovieList = await result;

        // Assert
        Assert.NotNull(resultMovieList);
        Assert.Equal(resultMovieList.Count, movieList.Count);
    }
}