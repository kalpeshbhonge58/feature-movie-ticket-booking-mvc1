using CineBookerEnterprise.Models.Domain;

namespace CineBookerEnterprise.Services.Interfaces
{
    public interface IShowService
    {
        Task<IEnumerable<Show>> GetAllShowsAsync();
        Task<Show?> GetShowByIdAsync(int id);
        Task<IEnumerable<Show>> GetShowsByMovieIdAsync(int movieId);
        Task CreateShowAsync(Show show);
        Task UpdateShowAsync(Show show);
        Task DeleteShowAsync(int id);
        Task<IEnumerable<Seat>> GetAvailableSeatsAsync(int showId);
        Task<IEnumerable<int>> GetBookedSeatIdsAsync(int showId);
    }
}
