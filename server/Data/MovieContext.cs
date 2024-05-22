using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data;

public class MovieContext : IdentityDbContext<User>
{
  private readonly IConfiguration _configuration; // contains connection string

  public DbSet<Movie>? Movies { get; set; }
  public DbSet<User>? Users { get; set; }
  public DbSet<UserMovie>? UserMovie { get; set; }

  public MovieContext(DbContextOptions<MovieContext> options, IConfiguration configuration) : base(options)
  {
    _configuration = configuration; //set a connection string
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<UserMovie>()
    .HasKey(um => new { um.UserId, um.MovideId });

    modelBuilder.Entity<UserMovie>()
    .HasOne(um => um.User)
    .WithMany(u => u.UserMovies)
    .HasForeignKey(um => um.UserId);

    modelBuilder.Entity<UserMovie>()
    .HasOne(um => um.Movie)
    .WithMany(m => m.UserMovies)
    .HasForeignKey(um => um.MovideId);
  }

}