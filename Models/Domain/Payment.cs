using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBookerEnterprise.Models.Domain
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking? Booking { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentTime { get; set; }

        [Required]
        public PaymentStatus Status { get; set; }

        public string TransactionId { get; set; } = string.Empty;
    }

    public enum PaymentStatus
    {
        Pending,
        Success,
        Failed
    }
}
