using CineBookerEnterprise.Models.ViewModels;
using CineBookerEnterprise.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CineBookerEnterprise.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IShowService _showService;

        public MoviesController(IMovieService movieService, IShowService showService)
        {
            _movieService = movieService;
            _showService = showService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null) return NotFound();

            var shows = await _showService.GetShowsByMovieIdAsync(id);

            var viewModel = new MovieDetailsViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                DurationMinutes = movie.DurationInMinutes,
                PosterUrl = movie.PosterUrl,
                AvailableShows = shows.Select(s => new ShowViewModel
                {
                    Id = s.Id,
                    MovieId = s.MovieId,
                    MovieTitle = s.Movie.Title,
                    ScreenId = s.ScreenId,
                    ScreenName = s.Screen.Name,
                    TheaterName = s.Screen.Theater.Name,
                    StartTime = s.StartTime,
                    Price = s.Price
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
