//join table
using server.Models;
namespace server.Models;

public class UserMovie
{
  public string UserId {get;set;}
  public User User {get;set;}

  public int MovieId {get;set;}
  public Movie Movie {get;set;}
  public string Description {get;set;}
  public string PosterPath {get;set;}
}