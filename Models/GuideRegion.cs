using System.ComponentModel.DataAnnotations.Schema;
using WebAppSafeJourney.Models.Enums;
namespace WebAppSafeJourney.Models
{
    public class GuideRegion
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public Guide? Guide { get; set; }
        public EgyptRegion Region { get; set; }
    }
}
