using System.ComponentModel.DataAnnotations.Schema;
using WebAppSafeJourney.Models.Enums;
namespace WebAppSafeJourney.Models
{
    public class GuideLanguage
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public Guide? Guide { get; set; }
        public string? Language { get; set; }
    }
}
