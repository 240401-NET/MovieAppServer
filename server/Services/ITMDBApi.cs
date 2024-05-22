using server.Models;

namespace server.Services;

public interface ITMDBApi
{
    public async Task<List<Movie>> GetUpcomingMovies(int currentPage);
    private async Task<string> GetCertification(int id);
    private Movie MapToMovie(TMDBMovieDto results, string certification);
    public async Task GetGenres();
    
}