using server.Models;
using server.Dto;
namespace server.Services;

public interface ITMDBApi
{
    Task<List<Movie>> GetUpcomingMovies(int currentPage);
    Task<string> GetCertification(int id);
    Movie MapToMovie(TMDBMovieDto results, string certification);
    Task<Movie> GetMovieInfo(int id);
    
    Task<List<Movie>> GetMoviesByGenre(string genre, int currentPage);
    Task<List<Movie>> GetMoviesByLanguage(string language, int currentPage);
    Task<Movie> GetMovieByTitle(string title);
}