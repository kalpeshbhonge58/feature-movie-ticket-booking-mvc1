using System.ComponentModel.DataAnnotations;

namespace CineBookerEnterprise.Models.ViewModels
{
    public class BookingRequestViewModel
    {
        [Required]
        public int ShowId { get; set; }

        [Required]
        public List<int> SeatIds { get; set; } = new();
    }

    public class PaymentViewModel
    {
        public int BookingId { get; set; }
        public decimal TotalPrice { get; set; }

        [Required]
        [Display(Name = "Card Holder Name")]
        public string CardHolderName { get; set; } = string.Empty;

        [Required]
        [CreditCard]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Expiration date must be in MM/YY format")]
        [Display(Name = "Expiration Date (MM/YY)")]
        public string ExpiryDate { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits")]
        public string CVV { get; set; } = string.Empty;
    }

    public class BookingHistoryViewModel
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string TheaterName { get; set; } = string.Empty;
        public string ScreenName { get; set; } = string.Empty;
        public DateTime ShowTime { get; set; }
        public DateTime BookingTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string> Seats { get; set; } = new();
    }

    public class BookingDetailsViewModel
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string TheaterName { get; set; } = string.Empty;
        public string ScreenName { get; set; } = string.Empty;
        public DateTime ShowTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string> Seats { get; set; } = new();
        public string TransactionId { get; set; } = string.Empty;
    }
}
