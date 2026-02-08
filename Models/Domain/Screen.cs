using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBookerEnterprise.Models.Domain
{
    public class Screen
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int TheaterId { get; set; }

        [ForeignKey("TheaterId")]
        public virtual Theater? Theater { get; set; }

        [Required]
        public int Capacity { get; set; }

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public virtual ICollection<Show> Shows { get; set; } = new List<Show>();
    }
}
