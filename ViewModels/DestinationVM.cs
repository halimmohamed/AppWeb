using System.ComponentModel.DataAnnotations;
using WebAppSafeJourney.Models.Enums;

namespace WebAppSafeJourney.ViewModels
{
    public class DestinationVM
    {
        // تم إضافة الـ Id لحل الـ Error
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty; // = string.Empty لحل تحذير الـ Null

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty; // = string.Empty لحل تحذير الـ Null

        public string? CoverImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Region is required")]
        public EgyptRegion Region { get; set; }
    }
}