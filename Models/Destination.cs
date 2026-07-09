using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppSafeJourney.Models.Enums;
using Microsoft.AspNetCore.Http; // لازم تضيف دي فوق

namespace WebAppSafeJourney.Models
{
    public class Destination
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public EgyptRegion Region { get; set; }
        [StringLength(5000)]
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [NotMapped] // معناها إن الحقل ده مش هيتحفظ كملف جوه الداتابيز، احنا هنحفظ مساره بس
        public IFormFile? ImageFile { get; set; }
        public ICollection<TripBooking>? TripBookings { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<GuideBooking>? GuideBookings { get; set; }
    }
}
