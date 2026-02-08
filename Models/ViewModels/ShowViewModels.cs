using System.ComponentModel.DataAnnotations;

namespace CineBookerEnterprise.Models.ViewModels
{
    public class ShowViewModel
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public int ScreenId { get; set; }
        public string ScreenName { get; set; } = string.Empty;
        public string TheaterName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
    }

    public class ShowFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Movie")]
        public int MovieId { get; set; }

        [Required]
        [Display(Name = "Screen")]
        public int ScreenId { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal Price { get; set; }
    }

    public class SeatViewModel
    {
        public int Id { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
        public bool IsBooked { get; set; }
    }

    public class SeatSelectionViewModel
    {
        public int ShowId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string TheaterName { get; set; } = string.Empty;
        public string ScreenName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
        public List<SeatViewModel> Seats { get; set; } = new();
    }
}
