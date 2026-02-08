namespace CineBookerEnterprise.Models.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalMovies { get; set; }
        public List<BookingHistoryViewModel> RecentBookings { get; set; } = new();
    }
}
