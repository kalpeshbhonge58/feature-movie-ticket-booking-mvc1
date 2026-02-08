using CineBookerEnterprise.Data;
using CineBookerEnterprise.Models.Domain;
using CineBookerEnterprise.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineBookerEnterprise.Services.Implementations
{
    public class ShowService : IShowService
    {
        private readonly ApplicationDbContext _context;

        public ShowService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Show>> GetAllShowsAsync()
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theater)
                .ToListAsync();
        }

        public async Task<Show?> GetShowByIdAsync(int id)
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theater)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Seats)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Show>> GetShowsByMovieIdAsync(int movieId)
        {
            return await _context.Shows
                .Include(s => s.Screen)
                .ThenInclude(sc => sc.Theater)
                .Where(s => s.MovieId == movieId && s.StartTime > DateTime.Now)
                .ToListAsync();
        }

        public async Task CreateShowAsync(Show show)
        {
            _context.Shows.Add(show);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateShowAsync(Show show)
        {
            _context.Shows.Update(show);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShowAsync(int id)
        {
            var show = await _context.Shows.FindAsync(id);
            if (show != null)
            {
                _context.Shows.Remove(show);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Seat>> GetAvailableSeatsAsync(int showId)
        {
            var show = await GetShowByIdAsync(showId);
            if (show == null) return new List<Seat>();

            var bookedSeatIds = await GetBookedSeatIdsAsync(showId);

            return await _context.Seats
                .Where(s => s.ScreenId == show.ScreenId && !bookedSeatIds.Contains(s.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetBookedSeatIdsAsync(int showId)
        {
            return await _context.BookingSeats
                .Where(bs => bs.Booking.ShowId == showId && bs.Booking.Status != BookingStatus.Cancelled)
                .Select(bs => bs.SeatId)
                .ToListAsync();
        }
    }
}
