namespace WebAppSafeJourney.ViewModels
{
    public class ReviewDisplayVM
    {
        public string TouristName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}