using System.ComponentModel.DataAnnotations;

namespace CineBookerEnterprise.Models.Domain
{
    public class Theater
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        public virtual ICollection<Screen> Screens { get; set; } = new List<Screen>();
    }
}
