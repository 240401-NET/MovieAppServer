using System.Text.Json;
using server.Models;

namespace server.Services;

public class MovieService
{
  private List<Genre> _genres;

  public MovieService()
  {
    _genres = LoadGenres();
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


}