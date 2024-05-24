using server.Dto;
using server.Services;
using server.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Moq;
using System.Text.Json;
using Moq.Protected;

namespace server.Tests;

public class TMDBServiceTests
{
    private readonly TMDBService _tmdbService;
    private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;
    private Mock<ITMDBApi> mockTMDBApi;
        public TMDBServiceTests()
        {
            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var mockConfig = new Mock<IConfiguration>();
            mockTMDBApi = new Mock<ITMDBApi>();

            // Set up the mock configuration to return test API key and token
        mockConfig.SetupGet(x => x["ApiKey"]).Returns("94a79b9432008c8e93ee3e30899485de");
        mockConfig.SetupGet(x => x["Token"]).Returns("eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI5NGE3OWI5NDMyMDA4YzhlOTNlZTNlMzA4OTk0ODVkZSIsInN1YiI6IjY2M2UyZDc4NzYwMTE1MmZhYjBmOWY5OCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.jk7TCktzMvtKZqde8D3JZMVA7HvjpUBtymj-F8gr4Ig");
        mockConfig.SetupGet(x => x["ConnectionString"]).Returns("Server=tcp:1942-p3.database.windows.net,1433;Initial Catalog=aDB;Persist Security Info=False;User ID=aDbUser;Password=a@dB0wn5r;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
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
    [Fact]
    public async Task TMDBApi_GetUpcomingMovies_ReturnsMovies()
    {
            // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new TMDBResponseDto
            {
                Results = new List<TMDBMovieDto>
                {
                    new TMDBMovieDto 
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
                    },
                    new TMDBMovieDto 
                    {
                        Id = 2,
                        Title = "Test Movie 2",
                        Overview = "Test Overview 2",
                        ReleaseDate = "06/06/2024",
                        PosterPath = "Test Poster Path 2",
                        VoteAverage = 5.0f,
                        VoteCount = 100,
                        GenreIds = new List<int> { 1, 2, 3 },
                        Adult = false,
                        BackdropPath = "Test Backdrop Path 2",
                        OriginalLanguage = "en",
                        OriginalTitle = "Test Original Title 2",
                        Video = false,
                        Popularity = 100.0f

                    }
                }
            }), Encoding.UTF8, new MediaTypeHeaderValue("application/json"))
        };
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

        

        // Act
        var movies = await _tmdbService.GetUpcomingMovies(1);

        // Assert
        Assert.NotNull(movies);
        Assert.Equal(2, movies.Count);
        Assert.Equal(1, movies[0].MovieId);
        Assert.Equal("Test Movie", movies[0].Title);
        Assert.Equal(2, movies[1].MovieId);
        Assert.Equal("Test Movie 2", movies[1].Title);
    }
    [Fact]
    public async Task TMDBApi_GetUpcomingMovies_ReturnsEmptyListWhenNoMovies()
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new TMDBResponseDto
            {
                Results = new List<TMDBMovieDto>()
            }), Encoding.UTF8, new MediaTypeHeaderValue("application/json"))
        };
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

        // Act
        var movies = await _tmdbService.GetUpcomingMovies(1);

        // Assert
        Assert.NotNull(movies);
        Assert.Empty(movies);
    }


    [Theory]
    [InlineData("english", "en")]
    [InlineData("spanish", "es")]
    [InlineData("french", "fa")]
    [InlineData("unsupported", "")]
    public async Task TMDBApi_GetMoviesByLanguage_ReturnsMovies_New(string language, string abbreviation)
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new TMDBResponseDto
            {
                Results = new List<TMDBMovieDto>
                {
                    new TMDBMovieDto 
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
                    },
                    // ... other movies ...
                }
            }), Encoding.UTF8, new MediaTypeHeaderValue("application/json"))
        };
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains($"with_original_language={abbreviation}")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);
        mockTMDBApi
            .Setup(api => api.GetCertification(It.IsAny<int>()))
            .ReturnsAsync("PG");
        mockTMDBApi
            .Setup(api => api.MapToMovie(It.IsAny<TMDBMovieDto>(), It.IsAny<string>()))
            .Returns(new Movie());
    
        // Act
        var movies = await _tmdbService.GetMoviesByLanguage(language, 1);
    
        // Assert
        Assert.NotNull(movies);
        Assert.NotEmpty(movies);
    }

  
   
 
}