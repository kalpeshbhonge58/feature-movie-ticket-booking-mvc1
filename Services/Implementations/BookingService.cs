using CineBookerEnterprise.Data;
using CineBookerEnterprise.Models.Domain;
using CineBookerEnterprise.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineBookerEnterprise.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(string userId, int showId, List<int> seatIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var show = await _context.Shows.FindAsync(showId);
                if (show == null) throw new Exception("Show not found");

                // Check if seats are already booked
                var alreadyBooked = await _context.BookingSeats
                    .AnyAsync(bs => bs.Booking.ShowId == showId && seatIds.Contains(bs.SeatId) && bs.Booking.Status != BookingStatus.Cancelled);

                if (alreadyBooked)
                {
                    throw new Exception("One or more seats are already booked.");
                }

                var booking = new Booking
                {
                    UserId = userId,
                    ShowId = showId,
                    BookingTime = DateTime.Now,
                    Status = BookingStatus.Pending,
                    TotalPrice = show.Price * seatIds.Count
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                foreach (var seatId in seatIds)
                {
                    _context.BookingSeats.Add(new BookingSeat
                    {
                        BookingId = booking.Id,
                        SeatId = seatId
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return booking;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Show)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Show)
                    .ThenInclude(s => s.Screen)
                        .ThenInclude(sc => sc.Theater)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.Show)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Show)
                    .ThenInclude(s => s.Screen)
                        .ThenInclude(sc => sc.Theater)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Show)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Payment)
                .OrderByDescending(b => b.BookingTime)
                .ToListAsync();
        }

        public async Task<bool> ProcessPaymentAsync(int bookingId, string transactionId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return false;

            var payment = new Payment
            {
                BookingId = bookingId,
                Amount = booking.TotalPrice,
                PaymentTime = DateTime.Now,
                Status = PaymentStatus.Success,
                TransactionId = transactionId
            };

            _context.Payments.Add(payment);
            booking.Status = BookingStatus.Confirmed;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Payments
                .Where(p => p.Status == PaymentStatus.Success)
                .SumAsync(p => p.Amount);
        }
    }
}
