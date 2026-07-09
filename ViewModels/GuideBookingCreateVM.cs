using System.ComponentModel.DataAnnotations;

namespace WebAppSafeJourney.ViewModels
{
    public class GuideBookingCreateVM
    {
        [Required]
        public int GuideId { get; set; }

        [Required(ErrorMessage = "Please select a destination")]
        [Display(Name = "Destination")]
        public int DestinationId { get; set; }

        [Required(ErrorMessage = "Please select a tour date")]
        [DataType(DataType.Date)]
        [Display(Name = "Tour Date")]
        public DateTime TourDate { get; set; } = DateTime.Today.AddDays(3);

        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
        [Display(Name = "Number of Days")]
        public int DurationDays { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Number of people must be between 1 and 100")]
        [Display(Name = "Number of People")]
        public int NumberOfPeople { get; set; } = 1;

        [StringLength(500)]
        [Display(Name = "Special Notes (Optional)")]
        public string? SpecialNotes { get; set; }
    }
}