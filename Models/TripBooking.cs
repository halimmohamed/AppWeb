using System.ComponentModel.DataAnnotations;
using WebAppSafeJourney.Models.Enums;

namespace WebAppSafeJourney.Models
{
    public class TripBooking
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public User? Tourist { get; set; }
        public int DestinationId { get; set; }
        public Destination? Destination { get; set; }
        public TripType TripType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Range(1, 100)]
        public int NumberOfPeople { get; set; }
        public string? SpecialNotes { get; set; }
        public TripBookingStatus Status { get; set; } = TripBookingStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
