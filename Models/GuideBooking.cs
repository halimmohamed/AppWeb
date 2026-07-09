using System.ComponentModel.DataAnnotations;
using WebAppSafeJourney.Models.Enums;

namespace WebAppSafeJourney.Models
{
    public class GuideBooking
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public User? Tourist { get; set; }
        public int GuideId { get; set; }
        public Guide? Guide { get; set; }
        public int DestinationId { get; set; }
        public Destination? Destination { get; set; }
        public DateTime TourDate { get; set; }
        [Range(1, 365)]
        public int DurationDays { get; set; }
        [Range(1, 100)]
        public int NumberOfPeople { get; set; }
        public string? SpecialNotes { get; set; }
        public GuideBookingStatus Status { get; set; } = GuideBookingStatus.Pending;
        public string? GuideResponseMessage { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Review? Review { get; set; }
    }
}
