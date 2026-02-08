using CineBookerEnterprise.Data;
using CineBookerEnterprise.Models.Domain;
using CineBookerEnterprise.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CineBookerEnterprise.Tests
{
    public class MovieServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task GetAllMoviesAsync_ReturnsAllMovies()
        {
            // Arrange
            var context = GetDbContext();
            context.Movies.Add(new Movie { Title = "Movie 1", Genre = "Action", DurationInMinutes = 120 });
            context.Movies.Add(new Movie { Title = "Movie 2", Genre = "Comedy", DurationInMinutes = 90 });
            await context.SaveChangesAsync();

            var service = new MovieService(context);

            // Act
            var result = await service.GetAllMoviesAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetMovieByIdAsync_ReturnsCorrectMovie()
        {
            // Arrange
            var context = GetDbContext();
            var movie = new Movie { Title = "Movie 1", Genre = "Action", DurationInMinutes = 120 };
            context.Movies.Add(movie);
            await context.SaveChangesAsync();

            var service = new MovieService(context);

            // Act
            var result = await service.GetMovieByIdAsync(movie.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Movie 1", result.Title);
        }
    }
}
