using WebAppSafeJourney.Models.Enums;

namespace WebAppSafeJourney.ViewModels
{
    public class DestinationDetailsVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public EgyptRegion Region { get; set; }
        public string? CoverImageUrl { get; set; }
        public List<ReviewDisplayVM> Reviews { get; set; } = new();
    }
}