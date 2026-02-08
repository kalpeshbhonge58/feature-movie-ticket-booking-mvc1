using CineBookerEnterprise.Models;
using CineBookerEnterprise.Models.ViewModels;
using CineBookerEnterprise.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CineBookerEnterprise.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IMovieService movieService, ILogger<HomeController> logger)
        {
            _movieService = movieService;
            _logger = logger;
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
