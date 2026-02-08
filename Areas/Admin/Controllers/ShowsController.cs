using CineBookerEnterprise.Models.Domain;
using CineBookerEnterprise.Models.ViewModels;
using CineBookerEnterprise.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CineBookerEnterprise.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ShowsController : Controller
    {
        private readonly IShowService _showService;
        private readonly IMovieService _movieService;
        private readonly ITheaterService _theaterService;

        public ShowsController(IShowService showService, IMovieService movieService, ITheaterService theaterService)
        {
            _showService = showService;
            _movieService = movieService;
            _theaterService = theaterService;
        }

        public async Task<IActionResult> Index()
        {
            var shows = await _showService.GetAllShowsAsync();
            var viewModel = shows.Select(s => new ShowViewModel
            {
                Id = s.Id,
                MovieId = s.MovieId,
                MovieTitle = s.Movie.Title,
                ScreenId = s.ScreenId,
                ScreenName = s.Screen.Name,
                TheaterName = s.Screen.Theater.Name,
                StartTime = s.StartTime,
                Price = s.Price
            }).ToList();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View(new ShowFormViewModel { StartTime = DateTime.Now.AddDays(1) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShowFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var show = new Show
                {
                    MovieId = model.MovieId,
                    ScreenId = model.ScreenId,
                    StartTime = model.StartTime,
                    Price = model.Price
                };
                await _showService.CreateShowAsync(show);
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDowns();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var show = await _showService.GetShowByIdAsync(id);
            if (show == null) return NotFound();

            var viewModel = new ShowFormViewModel
            {
                Id = show.Id,
                MovieId = show.MovieId,
                ScreenId = show.ScreenId,
                StartTime = show.StartTime,
                Price = show.Price
            };

            await PopulateDropDowns();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ShowFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var show = await _showService.GetShowByIdAsync(model.Id);
                if (show == null) return NotFound();

                show.MovieId = model.MovieId;
                show.ScreenId = model.ScreenId;
                show.StartTime = model.StartTime;
                show.Price = model.Price;

                await _showService.UpdateShowAsync(show);
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDowns();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var show = await _showService.GetShowByIdAsync(id);
            if (show == null) return NotFound();

            var viewModel = new ShowViewModel
            {
                Id = show.Id,
                MovieTitle = show.Movie.Title,
                StartTime = show.StartTime
            };
            return View(viewModel);
        }

        private async Task PopulateDropDowns()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            var theaters = await _theaterService.GetAllTheatersAsync();

            var screens = new List<Screen>();
            foreach (var theater in theaters)
            {
                screens.AddRange(await _theaterService.GetScreensByTheaterIdAsync(theater.Id));
            }

            ViewBag.Movies = new SelectList(movies, "Id", "Title");
            ViewBag.Screens = screens.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = $"{s.Theater?.Name} - {s.Name}"
            });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _showService.DeleteShowAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
