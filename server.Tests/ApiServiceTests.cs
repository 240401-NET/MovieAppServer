using server.Dto;
using server.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Moq;
namespace server.Tests;

public class TMDBServiceTests
{
    private readonly TMDBService _tmdbService;
    public TMDBServiceTests()
    {
        var httpClient = new HttpClient();
        var mockConfig = new Mock<IConfiguration>();
        _tmdbService = new TMDBService(httpClient, mockConfig.Object);
    }
    [Fact]
    public void UserService_MapDTOToMovie_ReturnsMovie()
    {
        //Arrange 
        var responseDTO = new TMDBMovieDto
        {
            Id = 1,
            Title = "Test Movie",
            Overview = "Test Overview",
            ReleaseDate = "06/06/2024",
            PosterPath = "Test Poster Path",
            VoteAverage = 5.0f,
            VoteCount = 100,
            GenreIds = new List<int> { 1, 2, 3 },
            Adult = false,
            BackdropPath = "Test Backdrop Path",
            OriginalLanguage = "en",
            OriginalTitle = "Test Original Title",
            Video = false,
            Popularity = 100.0f
        };
        string certification = "R";

        //Act
        var result = _tmdbService.MapToMovie(responseDTO, certification);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(responseDTO.Id, result.MovieId);
        Assert.Equal(responseDTO.Title, result.Title);
        Assert.Equal(responseDTO.Overview, result.MovieDescription);
        Assert.Equal(responseDTO.GenreIds[0].ToString(), result.Genre);
        Assert.Equal(DateTime.Parse(responseDTO.ReleaseDate), result.ReleaseDate);
        Assert.Equal(responseDTO.OriginalLanguage, result.MovieLanguage);
    }
}