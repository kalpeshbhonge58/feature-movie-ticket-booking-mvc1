using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBookerEnterprise.Models.Domain
{
    public class Show
    {
        public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie? Movie { get; set; }

        [Required]
        public int ScreenId { get; set; }

        [ForeignKey("ScreenId")]
        public virtual Screen? Screen { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
