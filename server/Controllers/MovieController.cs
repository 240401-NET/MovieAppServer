using server.Services;
using server.Models;
using server.Data;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;
[ApiController]
[Route("[controller]")]

public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IMovieRepository _movieRepository;
<<<<<<< HEAD
    private readonly TMDBService _apiService;
=======
    private readonly ITMDBApi _tmdbService;
>>>>>>> 5a0937003a9c94cbcee0a2f5df3f73dcc24f0dae

    public MovieController(IMovieService movieService, IUserService userService, IMovieRepository movieRepository, ITMDBApi tmdbService)
    {
        _movieService = movieService;
        _userService = userService;
        _movieRepository = movieRepository;
        _tmdbService = tmdbService;
    }
    //need to be finished by being implemented by the service

    [HttpGet("search/{title}")]
    public async Task<IActionResult> GetMovieByTitle(string title)
    {
        try{
            var movies = await _movieService.GetMovieByTitle(title);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("search/language={language}")]
    public async Task<IActionResult> GetMovieByLanguage(string language)
    {
        try{
            var movies = await _movieService.GetMovieByLanguage(language);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("search/genre={genre}")]
    public async Task<IActionResult> GetMovieByGenre(string genre)
    {
        try{
            var movies = await _movieService.GetMoviesByGenre(genre);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

<<<<<<< HEAD
    [HttpGet("movie/upcoming")]
    public async Task<IActionResult> GetUpcomingMovies(int currentPage)
    {
        try{
            var movies = await _apiService.GetUpcomingMovies(currentPage);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("movie/playing")]
    public async Task<IActionResult> GetNowPlayingMovies(int currentPage)
    {
        try{
            var movies = await _apiService.GetNowPlayingMovies(currentPage);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("movie/info/{movieId}")]
    public async Task<IActionResult> GetMovieInfo([FromBody] int movieId)
    {
        try{
            var movies = await _apiService.GetMovieInfo(movieId);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
=======
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
>>>>>>> 5a0937003a9c94cbcee0a2f5df3f73dcc24f0dae
        }
    }

}