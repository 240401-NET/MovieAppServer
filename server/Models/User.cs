using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace server.Models;

public class User : IdentityUser
{
  public int Age { get; set; }
  public Movie? Movie { get; set; }
  public ICollection<UserMovie> UserMovies { get; set; } = new List<UserMovie>();
}