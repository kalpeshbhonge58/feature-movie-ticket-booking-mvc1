using CineBookerEnterprise.Models.Domain;

namespace CineBookerEnterprise.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(string userId, int showId, List<int> seatIds);
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<bool> ProcessPaymentAsync(int bookingId, string transactionId);
        Task<decimal> GetTotalRevenueAsync();
    }
}
