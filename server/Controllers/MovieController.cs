using server.Services;
using server.Models;
using server.Data;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;
[ApiController]
[Route("[controller]")]

public class MovieController : ControllerBase
{
    // private readonly IMovieService _movieService;
    private readonly IUserService _userService;
    private readonly IMovieRepository _movieRepository;
    private readonly ITMDBApi _tmdbService;

    public MovieController( IUserService userService, IMovieRepository movieRepository, ITMDBApi tmdbService)
    {
        // _movieService = movieService;
        _userService = userService;
        _movieRepository = movieRepository;
        _tmdbService = tmdbService;
    }
    //need to be finished by being implemented by the service

    [HttpGet("search/{title}")]
    public async Task<IActionResult> GetMovieByTitle(string title)
    {
        try{
            var movies = await _tmdbService.GetMovieByTitle(title);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    

    [HttpGet("search/language={language}/{currentPage}")]
    public async Task<IActionResult> GetMovieByLanguage(string language, int currentPage)
    {
        try{
            var movies = await _tmdbService.GetMoviesByLanguage(language, currentPage);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("search/{genre}/{currentPage}")]
    public async Task<IActionResult> GetMovieByGenre(string genre, int currentPage)
    {
        try{
            var movies = await _tmdbService.GetMoviesByGenre(genre, currentPage);
            foreach (var movie in movies)
            {
            Console.WriteLine("By genre from controller: " + movie.Title);

            }
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    // [HttpGet("movie/upcoming")]
    // public async Task<IActionResult> GetUpcomingMovies()
    // {

    // }

    // [HttpGet("movie/playing")]
    // public async Task<IActionResult> GetNowPlayingMovies()
    // {
        
    // }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieInfo(int id)
    {
        try {
            var movie = await _tmdbService.GetMovieInfo(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}