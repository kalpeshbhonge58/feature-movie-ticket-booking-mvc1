using CineBookerEnterprise.Models.Domain;
using CineBookerEnterprise.Models.ViewModels;
using CineBookerEnterprise.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineBookerEnterprise.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ScreensController : Controller
    {
        private readonly ITheaterService _theaterService;

        public ScreensController(ITheaterService theaterService)
        {
            _theaterService = theaterService;
        }

        public async Task<IActionResult> Index(int theaterId)
        {
            var theater = await _theaterService.GetTheaterByIdAsync(theaterId);
            if (theater == null) return NotFound();

            var screens = await _theaterService.GetScreensByTheaterIdAsync(theaterId);
            var viewModel = screens.Select(s => new ScreenViewModel
            {
                Id = s.Id,
                Name = s.Name,
                TheaterId = s.TheaterId,
                TheaterName = theater.Name,
                Capacity = s.Seats.Count
            }).ToList();

            ViewBag.TheaterId = theaterId;
            ViewBag.TheaterName = theater.Name;
            return View(viewModel);
        }

        public IActionResult Create(int theaterId)
        {
            var model = new ScreenFormViewModel
            {
                TheaterId = theaterId,
                Rows = 5,
                SeatsPerRow = 10
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ScreenFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var screen = new Screen
                {
                    Name = model.Name,
                    TheaterId = model.TheaterId
                };
                await _theaterService.CreateScreenAsync(screen, model.Rows, model.SeatsPerRow);
                return RedirectToAction(nameof(Index), new { theaterId = model.TheaterId });
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var screen = await _theaterService.GetScreenByIdAsync(id);
            if (screen == null) return NotFound();

            var viewModel = new ScreenFormViewModel
            {
                Id = screen.Id,
                Name = screen.Name,
                TheaterId = screen.TheaterId
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ScreenFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var screen = await _theaterService.GetScreenByIdAsync(model.Id);
                if (screen == null) return NotFound();

                screen.Name = model.Name;

                await _theaterService.UpdateScreenAsync(screen);
                return RedirectToAction(nameof(Index), new { theaterId = screen.TheaterId });
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var screen = await _theaterService.GetScreenByIdAsync(id);
            if (screen == null) return NotFound();

            var viewModel = new ScreenViewModel
            {
                Id = screen.Id,
                Name = screen.Name,
                TheaterId = screen.TheaterId
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var screen = await _theaterService.GetScreenByIdAsync(id);
            if (screen == null) return NotFound();
            int theaterId = screen.TheaterId;
            await _theaterService.DeleteScreenAsync(id);
            return RedirectToAction(nameof(Index), new { theaterId = theaterId });
        }
    }
}
