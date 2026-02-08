using System.ComponentModel.DataAnnotations;

namespace CineBookerEnterprise.Models.ViewModels
{
    public class TheaterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int ScreenCount { get; set; }
    }

    public class TheaterFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;
    }

    public class ScreenViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TheaterId { get; set; }
        public string TheaterName { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }

    public class ScreenFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Theater")]
        public int TheaterId { get; set; }

        [Required]
        [Range(1, 500)]
        public int Rows { get; set; }

        [Required]
        [Range(1, 50)]
        public int SeatsPerRow { get; set; }
    }
}
