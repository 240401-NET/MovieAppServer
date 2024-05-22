using server.Services;
using server.Models;
using server.Data;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;
[ApiController]
[Route("[Controller]")]

public class MovieController : ControllerBase
{
    // private readonly IMovieService _movieService;
    private readonly IUserService _userService;
    private readonly IMovieRepository _movieRepository;
    private readonly ITMDBApi _tmdbservice;


    public MovieController(IUserService userService, IMovieRepository movieRepository, ITMDBApi tmdbService)
    {
        // _movieService = movieService;
        _userService = userService;
        _movieRepository = movieRepository;
        _tmdbservice = tmdbService;

    }
    //need to be finished by being implemented by the service

    [HttpGet("search/movie/title={title}")]
    public async Task<IActionResult> GetMovieByTitle(string title)
    {
        try{
            var movies = await _movieRepository.GetMovieByTitleAsync(title);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("search/movie/language={language}")]
    public async Task<IActionResult> GetMovieByLanguage(string language)
    {
        try{
            var movies = await _movieRepository.GetMovieByLanguageAsync(language);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("search/movie/genre={genre}")]
    public async Task<IActionResult> GetMovieByGenre(string genre)
    {
        try{
            var movies = await _movieRepository.GetMoviesByGenreAsync(genre);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("movie/upcoming")]
    public async Task<IActionResult> GetUpcomingMovies(int currentPage)
    {
        try{
            var movies = await _tmdbservice.GetUpcomingMovies(currentPage);
            return Ok(movies);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet("movie/playing")]
    public async Task<IActionResult> GetNowPlayingMovies(int currentPage)
    {
        try{
            var movies = await _tmdbservice.GetNowPlayingMovies(currentPage);
            return Ok(movies);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // [HttpGet("movie/info")]
    // public async Task<IActionResult> GetMovieInfo()
    // {

    // }

}