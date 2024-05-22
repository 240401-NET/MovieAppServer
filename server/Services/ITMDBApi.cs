using server.Models;
using server.Dto;

namespace server.Services;

public interface ITMDBApi
{
    public Task<List<TMDBMovieDto>> GetUpcomingMovies(int currentPage);
    public Task<List<TMDBMovieDto>> GetNowPlayingMovies(int currentPage);
    // private async Task<string> GetCertification(int id);
    // private Movie MapToMovie(TMDBMovieDto results, string certification);
    // public async Task GetGenres();
    
}