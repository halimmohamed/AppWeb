using System.ComponentModel.DataAnnotations;

namespace WebAppSafeJourney.ViewModels
{
    public class ReviewCreateVM
    {
        [Required]
        public int DestinationId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Comment must be between 5 and 500 characters")]
        public string Comment { get; set; } = string.Empty;
    }
}