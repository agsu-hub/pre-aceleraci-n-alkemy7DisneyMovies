using ChallengeBackendDisney.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace ChallengeBackendDisney.Data
{
    public class MovieContext : DbContext
    {

       

        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.HasDefaultSchema(Schema);
        //    modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        //}





        public DbSet<Character> characters { get; set; } = null!;
        public DbSet<Movie> movies { get; set; } = null!;
        public DbSet<Genre> genres { get; set; } = null!;

        //public DbSet<CharacterMovie> characterMovies { get; set; } = null!;
    }
}

