using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using server.Utils;

using server.Models;
using server.Dto;

namespace server.Services
{
  public class TMDBService : ITMDBApi
  {
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly string _token;

    private List<Genre> _genres;

    public static DateTime today = DateTime.Today;

    public TMDBService(HttpClient http, IConfiguration config)
    {
      _http = http;
      _apiKey = config["ApiKey"];
      _token = config["Token"];
    }

    public async Task<List<Movie>> GetUpcomingMovies(int currentPage)
    {
      string minDateRange = today.AddDays(-7).ToString("yyyy-MM-dd");
      string maxDateRange = today.AddDays(30).ToString("yyyy-MM-dd");

      try
      {
        var url = $"https://api.themoviedb.org/3/discover/movie?include_adult=false&include_video=false&with_original_language=en&language=en-US&page={currentPage}&sort_by=release_date.desc&with_release_type=2|3&release_date.gte={minDateRange}&release_date.lte={maxDateRange}";
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var result = await _http.GetAsync(url);
        if (result.IsSuccessStatusCode)
        {
          var content = await result.Content.ReadAsStringAsync();
          Console.WriteLine("Content: " + content);
          var response = JsonSerializer.Deserialize<TMDBResponseDto>(content);

          var movies = new List<Movie>();
          foreach (var movieItem in response.Results)
          {
            var certification = await GetCertification(movieItem.Id);
            var movie = MapToMovie(movieItem, certification);
            if (movie != null)
            {
              Console.WriteLine("Movie: " + movie.Title);
              movies.Add(movie);
            }
            else
            {
              Console.WriteLine("Movie mapping resulted in null for movie item ID: " + movieItem.Id);
            }
          }
          return movies;
        }
        else
        {
          Console.WriteLine($"Error: {result.StatusCode} - {await result.Content.ReadAsStringAsync()}");
        }
      }
      catch (Exception e)
      {
        Console.WriteLine("catch block " + e.Message);
        return null;
      }
      return null;
    }
    public async Task<List<Movie>> GetMoviesByLanguage(string language, int currentPage)
    {
      string abbreviation = "";
      string minDateRange = today.AddDays(-7).ToString("yyyy-MM-dd");
      string maxDateRange = today.AddDays(30).ToString("yyyy-MM-dd");

      if (language == "english")
      {
        abbreviation = "en";
      } else if (language == "spanish")
      {
        abbreviation = "es";
      } else if (language == "french")
      {
        abbreviation = "fa";
      }
      try
      {
        var url = $"https://api.themoviedb.org/3/discover/movie?include_adult=false&include_video=false&with_original_language={abbreviation}&page={currentPage}&sort_by=release_date.desc&with_release_type=2|3&release_date.gte={minDateRange}&release_date.lte={maxDateRange}";
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var result = await _http.GetAsync(url);
        if (result.IsSuccessStatusCode)
        {
          var content = await result.Content.ReadAsStringAsync();
          Console.WriteLine("Content: " + content);
          var response = JsonSerializer.Deserialize<TMDBResponseDto>(content);

          var movies = new List<Movie>();
          foreach (var movieItem in response.Results)
          {
            var certification = await GetCertification(movieItem.Id);
            var movie = MapToMovie(movieItem, certification);
            if (movie != null)
            {
              Console.WriteLine("Movie: " + movie.Title);
              movies.Add(movie);
            }
            else
            {
              Console.WriteLine("Movie mapping resulted in null for movie item ID: " + movieItem.Id);
            }
          }
          return movies;
        }
        else
        {
          Console.WriteLine($"Error: {result.StatusCode} - {await result.Content.ReadAsStringAsync()}");
        }
      }
      catch (Exception e)
      {
        Console.WriteLine("catch block " + e.Message);
        return null;
      }
      return null;
    }
     public List<Genre> LoadGenres()
    {
        try
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Responses", "genres.json");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' was not found.");
            }

            var json = File.ReadAllText(filePath);
            var response = JsonSerializer.Deserialize<GenreResponse>(json);

            return response?.genres ?? new List<Genre>();
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while loading genres: {e.Message}");
            return new List<Genre>();
        }
    }

    public int MapGenreToGenreId(string genre)
    {
        if (_genres == null)
        {
            _genres = LoadGenres();
            if (_genres == null || _genres.Count == 0)
            {
                throw new InvalidOperationException("Genres have not been loaded");
            }
        }

        Console.WriteLine("list size: " + _genres.Count);
        var matching = _genres.Find(g => g.name.Equals(genre, StringComparison.OrdinalIgnoreCase));
        if (matching != null)
        {
            return matching.id;
        }
        else
        {
            throw new ArgumentException($"Genre '{genre}' not found");
        }
    }


public class GenreResponse
{
    public List<Genre> genres { get; set; }
}


  public async Task<List<Movie>> GetMoviesByGenre(string genre, int currentPage)
  {
    string minDateRange = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
    string maxDateRange = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd");
    int genreId = MapGenreToGenreId(genre);
    Console.WriteLine("from get: " + genreId);
    try
    {
      var url = $"https://api.themoviedb.org/3/discover/movie?include_adult=false&with_genres={genreId}&include_video=false&with_original_language=en&language=en-US&page={currentPage}&sort_by=release_date.desc&with_release_type=2|3&release_date.gte={minDateRange}&release_date.lte={maxDateRange}";
      _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
      _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      var result = await _http.GetAsync(url);
      if (result.IsSuccessStatusCode)
      {
        var content = await result.Content.ReadAsStringAsync();
        Console.WriteLine("Content: " + content);
        var response = JsonSerializer.Deserialize<TMDBResponseDto>(content);

        var movies = new List<Movie>();
        foreach (var movieItem in response.Results)
        {
          var certification = await GetCertification(movieItem.Id);
          var movie = MapToMovie(movieItem, certification);
          if (movie != null)
          {
            Console.WriteLine("Movie: " + movie.Title);
            movies.Add(movie);
          }
          else
          {
            Console.WriteLine("Movie mapping resulted in null for movie item ID: " + movieItem.Id);
          }
        }
        return movies;
      }
      else
      {
        Console.WriteLine($"Error: {result.StatusCode} - {await result.Content.ReadAsStringAsync()}");
      }
    }
    catch (Exception e)
    {
      Console.WriteLine("catch block " + e.Message);
      return null;
    }
    return null;
  }

  //GetMovieByTitle(string title)
  public async Task<Movie> GetMovieByTitle(string title)
    {
        try
        {
            var url = $"https://api.themoviedb.org/3/search/movie?query={Uri.EscapeDataString(title)}&language=en-US";
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await _http.GetAsync(url);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<SearchMovieResponse>(content);

                return response?.results.FirstOrDefault();
            }
            else
            {
                Console.WriteLine($"Error: {result.StatusCode} - {await result.Content.ReadAsStringAsync()}");
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while fetching the movie: " + e.Message);
            return null;
        }
    }
    public class SearchMovieResponse
{
    public List<Movie> results { get; set; }
}
  //GetMovieByLanguage(string language)
  //GetMovieByGenre(string genre)

  public async Task<string> GetCertification(int id)
  {
    try
    {
      var url = $"https://api.themoviedb.org/3/movie/{id}/release_dates?api_key={_apiKey}";

      using var request = new HttpRequestMessage(HttpMethod.Get, url);
      request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      var result = await _http.SendAsync(request);
      if (result.IsSuccessStatusCode)
      {
        var content = await result.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(content);
        var results = jsonDoc.RootElement.GetProperty("results");

        foreach (var resultElement in results.EnumerateArray())
        {
          var country = resultElement.GetProperty("iso_3166_1").GetString();
          if (country == "US")
          {
            var releaseDates = resultElement.GetProperty("release_dates");
            foreach (var releaseDate in releaseDates.EnumerateArray())
            {
              if (releaseDate.TryGetProperty("certification", out var certification))
              {
                return certification.GetString();
              }
            }
          }
        }
      }
      else
      {
        Console.WriteLine($"Error: {result.StatusCode} - {await result.Content.ReadAsStringAsync()}");
      }
    }
    catch (Exception e)
    {
      Console.WriteLine("Error getting certification: " + e.Message);
    }
    return null;
  }

  public Movie MapToMovie(TMDBMovieDto results, string certification)
  {
    string genre = "unknown";

    if (results.GenreIds != null && results.GenreIds.Count > 0)
    {
      genre = results.GenreIds[0].ToString();
    }

    return new Movie
    {
      MovieId = results.Id,
      Title = results.Title,
      ReleaseDate = DateTime.Parse(results.ReleaseDate),
      Genre = genre,
      MovieLanguage = results.OriginalLanguage,
      Rating = certification,
      MovieDescription = results.Overview,
      PosterPath = results.PosterPath,
      NowPlaying = DateTime.Parse(results.ReleaseDate) >= today,
      IsFavorited = false,
      PurchasedTickets = false
    };
    //will still need to assign isFavorited, purchasedTickets, and nowPlaying
  }

  public async Task GetGenres()
  {
    JsonSerializerOptions options = new()
    {
      WriteIndented = true
    };
    try
    {
      var result = await _http.GetAsync($"https://api.themoviedb.org/3/genre/movie/list?api_key={_apiKey}");
      if (result.IsSuccessStatusCode)
      {
        var content = await result.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(content);
        var formatted = JsonSerializer.Serialize(jsonDoc, options);
        await File.WriteAllTextAsync("Responses\\genres.json", formatted);
      }
      else
      {
        Console.WriteLine($"Error: {result.StatusCode}");
      }
    }
    catch (Exception e)
    {
      Console.WriteLine("catch block " + e.Message);
    }
  }

  //method to map genre to genreid from Responses



  public async Task<Movie> GetMovieInfo(int id)
  {
    //call cast method as well
    try
    {
      string url = $"https://api.themoviedb.org/3/movie/{id}?api_key={_apiKey}";
      var result = await _http.GetAsync(url);
      if (result.IsSuccessStatusCode)
      {
        var content = await result.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<TMDBMovieDto>(content);
        Console.WriteLine("response: " + response);
        var certification = await GetCertification(id);
        Movie movie = MapToMovie(response, certification);

        Console.WriteLine("Movie: " + movie.Title);
        return movie;
      }
      else
      {
        Console.WriteLine($"Error: {result.StatusCode} - {await result.Content.ReadAsStringAsync()}");
        return null;
      }
    }
    catch (Exception e)
    {
      Console.WriteLine("Error getting movie info: " + e.Message);
      throw;
    }
  }


}
}
