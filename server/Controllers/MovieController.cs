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
    // private readonly IUserService _userService;
    // private readonly IMovieRepository _movieRepository;


    // public MovieController(IMovieService movieService, IUserService userService, IMovieRepository movieRepository)
    // {
    //     _movieService = movieService;
    //     _userService = userService;
    //     _movieRepository = movieRepository;

    // }
    // //need to be finished by being implemented by the service

    // [HttpGet("search/movie/{title}")]
    // public async Task<IActionResult> GetMovieByTitle(string title)
    // {
    //     try{
    //         var movies = await _movieRepository.GetMovieByTitleAsync(title);
    //         return Ok(movies);
    //     }
    //     catch(Exception e)
    //     {
    //         return BadRequest(e.Message);
    //     }

    // }

    // [HttpGet("search/movie/{language}")]
    // public async Task<IActionResult> GetMovieByLanguage(string language)
    // {
    //     try{
    //         var movies = await _movieRepository.GetMovieByLanguageAsync(language);
    //         return Ok(movies);
    //     }
    //     catch(Exception e)
    //     {
    //         return BadRequest(e.Message);
    //     }

    // }

    // [HttpGet("search/movie/{genre}")]
    // public async Task<IActionResult> GetMovieByGenre(string genre)
    // {
    //     try{
    //         var movies = await _movieRepository.GetMoviesByGenreAsync(genre);
    //         return Ok(movies);
    //     }
    //     catch(Exception e)
    //     {
    //         return BadRequest(e.Message);
    //     }

    // }

    // [HttpGet("movie/upcoming")]
    // public async Task<IActionResult> GetUpcomingMovies()
    // {

    // }

    // [HttpGet("movie/playing")]
    // public async Task<IActionResult> GetNowPlayingMovies()
    // {

    // }

    // [HttpGet("movie/info")]
    // public async Task<IActionResult> GetMovieInfo()
    // {

    // }

}