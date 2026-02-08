using CineBookerEnterprise.Data;
using CineBookerEnterprise.Models.Domain;
using CineBookerEnterprise.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineBookerEnterprise.Services.Implementations
{
    public class TheaterService : ITheaterService
    {
        private readonly ApplicationDbContext _context;

        public TheaterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Theater>> GetAllTheatersAsync()
        {
            return await _context.Theaters.Include(t => t.Screens).ToListAsync();
        }

        public async Task<Theater?> GetTheaterByIdAsync(int id)
        {
            return await _context.Theaters.FindAsync(id);
        }

        public async Task CreateTheaterAsync(Theater theater)
        {
            _context.Theaters.Add(theater);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTheaterAsync(Theater theater)
        {
            _context.Theaters.Update(theater);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTheaterAsync(int id)
        {
            var theater = await _context.Theaters.FindAsync(id);
            if (theater != null)
            {
                _context.Theaters.Remove(theater);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Screen>> GetScreensByTheaterIdAsync(int theaterId)
        {
            return await _context.Screens.Include(s => s.Seats).Where(s => s.TheaterId == theaterId).ToListAsync();
        }

        public async Task<Screen?> GetScreenByIdAsync(int id)
        {
            return await _context.Screens.FindAsync(id);
        }

        public async Task CreateScreenAsync(Screen screen, int rows = 5, int seatsPerRow = 10)
        {
            _context.Screens.Add(screen);
            await _context.SaveChangesAsync();

            for (int i = 0; i < rows; i++)
            {
                char row = (char)('A' + i);
                for (int j = 1; j <= seatsPerRow; j++)
                {
                    _context.Seats.Add(new Seat { Row = row.ToString(), Number = j, ScreenId = screen.Id });
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateScreenAsync(Screen screen)
        {
            _context.Screens.Update(screen);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteScreenAsync(int id)
        {
            var screen = await _context.Screens.FindAsync(id);
            if (screen != null)
            {
                _context.Screens.Remove(screen);
                await _context.SaveChangesAsync();
            }
        }
    }
}
