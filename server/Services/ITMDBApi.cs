using server.Models;
using server.Dto;
namespace server.Services;

public interface ITMDBApi
{
    Task<List<Movie>> GetUpcomingMovies(int currentPage);
    Task<string> GetCertification(int id);
    Movie MapToMovie(TMDBMovieDto results, string certification);
    
}