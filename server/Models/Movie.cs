using System;
using System.Collections.Generic;
using server.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models;

public class Movie
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int MovieId { get; set; }
  [Required(ErrorMessage = "Title is required")]
  public string? Title { get; set; }
  [Required(ErrorMessage = "Release date is required")]

  public DateTime? ReleaseDate { get; set; }
  [Required(ErrorMessage = "Genre is required")]

  public string? Genre { get; set; }

  public bool? IsFavorited { get; set; }

  public bool? PurchasedTickets { get; set; }
  [Required(ErrorMessage = "Movie Language is required")]

  public string? MovieLanguage { get; set; }
  [Required(ErrorMessage = "Rating is required")]

  public string? Rating { get; set; }

  public bool? NowPlaying { get; set; }
  public string PosterPath {get;set;}

  public string? MovieDescription { get; set; }
  public ICollection<UserMovie> UserMovies { get; set; } = new List<UserMovie>();

}