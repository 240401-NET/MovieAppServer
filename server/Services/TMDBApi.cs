using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using server.Models;
using server.Dto;

namespace server.Services
{
  public class TMDBService : ITMDBApi
  {
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly string _token;
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

    public  Movie MapToMovie(TMDBMovieDto results, string certification)
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
        MovieDescription = results.Overview
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

        

        // public async Task<Movie> GetByTitle(string title)
        // {

        // }
    }
}
