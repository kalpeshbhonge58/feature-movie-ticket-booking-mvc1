using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBookerEnterprise.Models.Domain
{
    public class Seat
    {
        public int Id { get; set; }

        [Required]
        [StringLength(5)]
        public string Row { get; set; } = string.Empty;

        [Required]
        public int Number { get; set; }

        [Required]
        public int ScreenId { get; set; }

        [ForeignKey("ScreenId")]
        public virtual Screen? Screen { get; set; }
    }
}
