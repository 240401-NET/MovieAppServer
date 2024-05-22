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
    private readonly ITMDBApi _tmdbService;

    public MovieController(IMovieService movieService, IMovieRepository movieRepository, ITMDBApi tmdbService)
    {
        _movieService = movieService;
        _movieRepository = movieRepository;
        _tmdbService = tmdbService;
    }

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
            var movies = await _tmdbService.GetUpcomingMovies(currentPage);
            return Ok(movies);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

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