using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace server.Dto;

public class TMDBResponseDto
{
  [JsonPropertyName("results")]
  public List<TMDBMovieDto> Results { get; set; }
}