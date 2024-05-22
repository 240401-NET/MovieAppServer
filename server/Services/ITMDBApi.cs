using server.Models;
using server.Dto;

namespace server.Services;

public interface ITMDBApi
{
    public Task<List<Movie>> GetUpcomingMovies(int currentPage);
    public Task<List<Movie>> GetNowPlayingMovies(int currentPage);
    public Task<string> GetCertification(int id);
    public Movie MapToMovie(TMDBMovieDto results, string certification);
    public Task GetGenres();
    
}