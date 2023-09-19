using Microsoft.Extensions.Logging;
using Movies.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Infrastructure.Data
{
    public class MovieContextSeed
    {
        public static async Task SeedAsync(MovieContext movieContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailabilty = retry.Value;
            try
            {
                await movieContext.Database.EnsureCreatedAsync();
                if (!movieContext.Movies.Any())
                {
                    movieContext.Movies.AddRange(GetMovies());
                    await movieContext.SaveChangesAsync();  
                }
            }
            catch (Exception ex)
            {
                if(retryForAvailabilty < 3)
                {
                    retryForAvailabilty++;
                    var log = loggerFactory.CreateLogger<MovieContextSeed>();
                    log.LogError($"Exception occured while connecting: {ex.Message}");
                    await SeedAsync(movieContext, loggerFactory, retryForAvailabilty);
                }
            }
        }

        private static IEnumerable<Movie> GetMovies()
        {
            return new List<Movie>()
            {
                new Movie() {MovieName = "Spider-Man", DirectorName = "James Cameron", ReleaseDate = "2002"},
                new Movie() {MovieName = "Spider-Man 2", DirectorName = "James Cameron", ReleaseDate = "2004"},
                new Movie() {MovieName = "Spider-Man 3", DirectorName = "James Cameron", ReleaseDate = "2007"},
                new Movie() {MovieName = "The Amazing Spider-Man", DirectorName = "James Cameron", ReleaseDate = "2012"}
            };
        }
    }
}
