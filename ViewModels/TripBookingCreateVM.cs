using System.ComponentModel.DataAnnotations;
using WebAppSafeJourney.Models.Enums;

namespace WebAppSafeJourney.ViewModels
{
    public class TripBookingCreateVM
    {
        [Required]
        public int DestinationId { get; set; }

        [Required(ErrorMessage = "Trip type is required")]
        public TripType TripType { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(1);

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(3);

        [Required(ErrorMessage = "Number of people is required")]
        [Range(1, 100, ErrorMessage = "Number of people must be between 1 and 100")]
        public int NumberOfPeople { get; set; } = 1;

        [MaxLength(500, ErrorMessage = "Special notes cannot exceed 500 characters")]
        public string? SpecialNotes { get; set; }
    }
}