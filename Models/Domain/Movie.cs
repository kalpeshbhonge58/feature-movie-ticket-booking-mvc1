using System.ComponentModel.DataAnnotations;

namespace CineBookerEnterprise.Models.Domain
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? PosterUrl { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }

        [Required]
        public string Genre { get; set; } = string.Empty;

        public virtual ICollection<Show> Shows { get; set; } = new List<Show>();
    }
}
