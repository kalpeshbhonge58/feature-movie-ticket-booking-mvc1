using CineBookerEnterprise.Models.Domain;
using CineBookerEnterprise.Models.ViewModels;
using CineBookerEnterprise.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineBookerEnterprise.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            var viewModel = movies.Select(m => new MovieListItemViewModel
            {
                Id = m.Id,
                Title = m.Title,
                Genre = m.Genre,
                PosterUrl = m.PosterUrl,
                DurationMinutes = m.DurationInMinutes,
                Description = m.Description
            }).ToList();
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View(new MovieFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var movie = new Movie
                {
                    Title = model.Title,
                    Description = model.Description,
                    Genre = model.Genre,
                    DurationInMinutes = model.DurationMinutes,
                    PosterUrl = model.PosterUrl
                };
                await _movieService.CreateMovieAsync(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null) return NotFound();

            var viewModel = new MovieFormViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                DurationMinutes = movie.DurationInMinutes,
                PosterUrl = movie.PosterUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var movie = await _movieService.GetMovieByIdAsync(model.Id);
                if (movie == null) return NotFound();

                movie.Title = model.Title;
                movie.Description = model.Description;
                movie.Genre = model.Genre;
                movie.DurationInMinutes = model.DurationMinutes;
                movie.PosterUrl = model.PosterUrl;

                await _movieService.UpdateMovieAsync(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null) return NotFound();

            var viewModel = new MovieListItemViewModel
            {
                Id = movie.Id,
                Title = movie.Title
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movieService.DeleteMovieAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
