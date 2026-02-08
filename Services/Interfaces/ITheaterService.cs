using CineBookerEnterprise.Models.Domain;

namespace CineBookerEnterprise.Services.Interfaces
{
    public interface ITheaterService
    {
        Task<IEnumerable<Theater>> GetAllTheatersAsync();
        Task<Theater?> GetTheaterByIdAsync(int id);
        Task CreateTheaterAsync(Theater theater);
        Task UpdateTheaterAsync(Theater theater);
        Task DeleteTheaterAsync(int id);

        Task<IEnumerable<Screen>> GetScreensByTheaterIdAsync(int theaterId);
        Task<Screen?> GetScreenByIdAsync(int id);
        Task CreateScreenAsync(Screen screen, int rows = 5, int seatsPerRow = 10);
        Task UpdateScreenAsync(Screen screen);
        Task DeleteScreenAsync(int id);
    }
}
