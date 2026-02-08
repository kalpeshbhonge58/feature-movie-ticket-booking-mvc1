using System.ComponentModel.DisplayName;
using System.ComponentModel.DataAnnotations;

namespace CineBookerEnterprise.Models.ViewModels
{
    public class MovieListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class MovieDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
        public List<ShowViewModel> AvailableShows { get; set; } = new();
    }

    public class MovieFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Genre { get; set; } = string.Empty;

        [Required]
        [Range(1, 500)]
        [Display(Name = "Duration (minutes)")]
        public int DurationMinutes { get; set; }

        [Required]
        [Url]
        [Display(Name = "Poster URL")]
        public string PosterUrl { get; set; } = string.Empty;
    }
}
