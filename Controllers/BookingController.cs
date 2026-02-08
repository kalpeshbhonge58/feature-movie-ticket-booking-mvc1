using CineBookerEnterprise.Models.ViewModels;
using CineBookerEnterprise.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CineBookerEnterprise.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IShowService _showService;
        private readonly IBookingService _bookingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(IShowService showService, IBookingService bookingService, UserManager<ApplicationUser> userManager)
        {
            _showService = showService;
            _bookingService = bookingService;
            _userManager = userManager;
        }

        public async Task<IActionResult> SelectSeats(int showId)
        {
            var show = await _showService.GetShowByIdAsync(showId);
            if (show == null) return NotFound();

            var viewModel = new SeatSelectionViewModel
            {
                ShowId = show.Id,
                MovieTitle = show.Movie.Title,
                TheaterName = show.Screen.Theater.Name,
                ScreenName = show.Screen.Name,
                StartTime = show.StartTime,
                Price = show.Price,
                Seats = show.Screen.Seats.Select(s => new SeatViewModel
                {
                    Id = s.Id,
                    Row = s.Row,
                    Number = s.Number
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetBookedSeats(int showId)
        {
            var bookedSeatIds = await _showService.GetBookedSeatIdsAsync(showId);
            return Json(bookedSeatIds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequestViewModel request)
        {
            if (request == null || request.SeatIds == null || !request.SeatIds.Any())
            {
                return BadRequest("Invalid booking request.");
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null) return Unauthorized();

                var booking = await _bookingService.CreateBookingAsync(userId, request.ShowId, request.SeatIds);
                return Json(new { success = true, bookingId = booking.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Payment(int bookingId)
        {
            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            if (booking == null) return NotFound();

            var viewModel = new PaymentViewModel
            {
                BookingId = booking.Id,
                TotalPrice = booking.TotalPrice
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(PaymentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Payment", viewModel);
            }

            // Simulate payment processing
            var transactionId = "TXN-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            var success = await _bookingService.ProcessPaymentAsync(viewModel.BookingId, transactionId);

            if (success)
            {
                return RedirectToAction(nameof(Confirmation), new { id = viewModel.BookingId });
            }

            return View("PaymentError");
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();

            var viewModel = new BookingDetailsViewModel
            {
                Id = booking.Id,
                MovieTitle = booking.Show.Movie.Title,
                TheaterName = booking.Show.Screen.Theater.Name,
                ScreenName = booking.Show.Screen.Name,
                ShowTime = booking.Show.StartTime,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status.ToString(),
                Seats = booking.BookingSeats.Select(bs => $"{bs.Seat.Row}{bs.Seat.Number}").ToList(),
                TransactionId = booking.Payment?.TransactionId ?? "N/A"
            };

            return View(viewModel);
        }

        public async Task<IActionResult> MyBookings()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();
            var bookings = await _bookingService.GetUserBookingsAsync(userId);

            var viewModel = bookings.Select(b => new BookingHistoryViewModel
            {
                Id = b.Id,
                MovieTitle = b.Show.Movie.Title,
                TheaterName = b.Show.Screen.Theater.Name,
                ScreenName = b.Show.Screen.Name,
                ShowTime = b.Show.StartTime,
                BookingTime = b.BookingTime,
                TotalPrice = b.TotalPrice,
                Status = b.Status.ToString(),
                Seats = b.BookingSeats.Select(bs => $"{bs.Seat.Row}{bs.Seat.Number}").ToList()
            }).ToList();

            return View(viewModel);
        }
    }
}
