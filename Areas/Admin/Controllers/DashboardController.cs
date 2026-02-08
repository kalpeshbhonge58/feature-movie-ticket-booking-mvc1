using CineBookerEnterprise.Models.ViewModels;
using CineBookerEnterprise.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineBookerEnterprise.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IMovieService _movieService;

        public DashboardController(IBookingService bookingService, IMovieService movieService)
        {
            _bookingService = bookingService;
            _movieService = movieService;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            var viewModel = new DashboardViewModel
            {
                TotalRevenue = await _bookingService.GetTotalRevenueAsync(),
                TotalMovies = (await _movieService.GetAllMoviesAsync()).Count(),
                RecentBookings = bookings.Take(10).Select(b => new BookingHistoryViewModel
                {
                    Id = b.Id,
                    MovieTitle = b.Show.Movie.Title,
                    BookingTime = b.BookingTime,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status.ToString()
                }).ToList()
            };
            return View(viewModel);
        }
    }
}
