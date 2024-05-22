namespace server.Dto;

public class FavoritedMovieDto
{
  public string Username {get;set;}
  public string MovieTitle {get;set;}
  public int MovieId {get;set;} //since frontend will have access to Id, pass that as well
  public string PosterPath {get;set;}
  public string Description {get;set;}
}