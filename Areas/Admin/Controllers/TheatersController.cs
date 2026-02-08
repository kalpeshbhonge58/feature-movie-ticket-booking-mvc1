using CineBookerEnterprise.Models.Domain;
using CineBookerEnterprise.Models.ViewModels;
using CineBookerEnterprise.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineBookerEnterprise.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TheatersController : Controller
    {
        private readonly ITheaterService _theaterService;

        public TheatersController(ITheaterService theaterService)
        {
            _theaterService = theaterService;
        }

        public async Task<IActionResult> Index()
        {
            var theaters = await _theaterService.GetAllTheatersAsync();
            var viewModel = theaters.Select(t => new TheaterViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Location = t.Location,
                ScreenCount = t.Screens.Count
            }).ToList();
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View(new TheaterFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TheaterFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var theater = new Theater
                {
                    Name = model.Name,
                    Location = model.Location
                };
                await _theaterService.CreateTheaterAsync(theater);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var theater = await _theaterService.GetTheaterByIdAsync(id);
            if (theater == null) return NotFound();

            var viewModel = new TheaterFormViewModel
            {
                Id = theater.Id,
                Name = theater.Name,
                Location = theater.Location
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TheaterFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var theater = await _theaterService.GetTheaterByIdAsync(model.Id);
                if (theater == null) return NotFound();

                theater.Name = model.Name;
                theater.Location = model.Location;

                await _theaterService.UpdateTheaterAsync(theater);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var theater = await _theaterService.GetTheaterByIdAsync(id);
            if (theater == null) return NotFound();

            var viewModel = new TheaterViewModel
            {
                Id = theater.Id,
                Name = theater.Name
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _theaterService.DeleteTheaterAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
