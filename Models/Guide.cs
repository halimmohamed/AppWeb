using System.ComponentModel.DataAnnotations.Schema;
using WebAppSafeJourney.Models.Enums;

namespace WebAppSafeJourney.Models
{
    public class Guide
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        public string? LicenseNumber { get; set; }
        public string? NationalId { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? NationalIdImageUrl { get; set; }
        public string? Bio { get; set; }
        public GuideStatus Status { get; set; } = GuideStatus.Pending;
        public string? RejectionReason { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public ICollection<GuideLanguage>? Languages { get; set; }
        public ICollection<GuideRegion>? CoveredRegions { get; set; }
        public ICollection<GuideBooking>? GuideBookings { get; set; }
    }
}
