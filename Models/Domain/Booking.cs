using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBookerEnterprise.Models.Domain
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int ShowId { get; set; }

        [ForeignKey("ShowId")]
        public virtual Show? Show { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public DateTime BookingTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public BookingStatus Status { get; set; }

        public virtual ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
        public virtual Payment? Payment { get; set; }
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}
