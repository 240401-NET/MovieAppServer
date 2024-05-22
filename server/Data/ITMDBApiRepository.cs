using server.Models;

namespace server.Data;

public interface ITMDBApiRepository
{
  Task AddMovieToDb(Movie movie);
}