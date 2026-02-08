using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBookerEnterprise.Models.Domain
{
    public class BookingSeat
    {
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking? Booking { get; set; }

        [Required]
        public int SeatId { get; set; }

        [ForeignKey("SeatId")]
        public virtual Seat? Seat { get; set; }
    }
}
