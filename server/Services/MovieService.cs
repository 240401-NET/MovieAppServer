using System.Text.Json;
using server.Models;
using server.Services;

namespace server.Services;

public class MovieService
{
  private List<Genre> _genres;
  private readonly IMovieRepository _movieRepository;


  public MovieService(IMovieRepository movieRepository)
  {
    _genres = LoadGenres();
    _movieRepository = movieRepository;
  }

//reading from local file -> list
  public static List<Genre> LoadGenres()
  {
    try
    {
      string path = "Responses\\genres.json";
      var jsonString = File.ReadAllText(path);
      return JsonSerializer.Deserialize<List<Genre>>(jsonString) ?? new List<Genre>();
    }
    catch (Exception e)
    {
      Console.WriteLine("Error loading genre file: " + e.Message);
      throw;
    }
  }
  public int MapGenreToGenreId(string genre)
  {
    if (_genres == null)
    {
      throw new InvalidOperationException("Genres have not been loaded");
    }
    var matching = _genres.Find(g => g.name.Equals(genre, StringComparison.OrdinalIgnoreCase));
    if (matching != null)
    {
      return matching.id;
    }
    else {
      throw new ArgumentException($"Genre '{genre}' not found");
    }
  }

  public async Task<Movie> GetMovieByTitle(string title)
  {
    return await _movieRepository.GetMovieByTitleAsync(title);
  }

    public async Task<List<Movie>> GetMovieByLanguage(string language)
  {
    return await _movieRepository.GetMovieByLanguageAsync(language);
  }

    public async Task<List<Movie>> GetMovieByGenre(string genre)
  {
    return await _movieRepository.GetMovieByGenreAsync(genre);
  }
}