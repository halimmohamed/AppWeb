using System.ComponentModel.DataAnnotations;

namespace WebAppSafeJourney.Models.Enums
{
    public enum UserRole { Tourist, Guide, Admin }
    public enum GuideStatus { Pending, Approved, Rejected }
    public enum TripBookingStatus { Pending, Confirmed, Completed, Cancelled }
    public enum GuideBookingStatus { Pending, Confirmed, Rejected, Cancelled, Completed }

    public enum EgyptRegion
    {
        [Display(Name = "Greater Cairo")] GreaterCairo,
        [Display(Name = "Upper Egypt")] UpperEgypt,
        [Display(Name = "Lower Egypt")] LowerEgypt,
        [Display(Name = "Red Sea Coast")] RedSeaCoast,
        [Display(Name = "Sinai Peninsula")] SinaiPeninsula,
        [Display(Name = "Mediterranean Coast")] MediterraneanCoast,
        [Display(Name = "Western Desert")] WesternDesert,
        [Display(Name = "Eastern Desert")] EasternDesert,
        [Display(Name = "Nile Delta")] NileDelta
    }

    public enum TripType
    {
        Cultural, Historical, Adventure, Beach, DesertSafari,
        NileCruise, Religious, Photography, Family
    }
}
