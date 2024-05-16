using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data;

public class MovieContext : IdentityDbContext<User>
{
  private readonly IConfiguration _configuration; // contains connection string

  public DbSet<Movie>? Movie { get; set; }
  public DbSet<User>? User { get; set; }

  public MovieContext(DbContextOptions<MovieContext> options, IConfiguration configuration) : base(options)
  {
    _configuration = configuration; //set a connection string
  }

}