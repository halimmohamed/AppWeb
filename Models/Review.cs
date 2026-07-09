using System.ComponentModel.DataAnnotations;

namespace WebAppSafeJourney.Models
{
    public class Review
    {
        public int Id { get; set; }

        // ربط التقييم بالسائح اللي كتبه
        public int TouristId { get; set; }
        public User? Tourist { get; set; }

        // ربط التقييم بالمزار السياحي
        public int DestinationId { get; set; }
        public Destination? Destination { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Please enter your comment")]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}