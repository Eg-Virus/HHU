
namespace AirObservationSystem.HHU.Core.Model.User
{
    public class UserRegion
    {
        public int UserRegionID { get; set; }
        public int UserID { get; set; }
        public int RegionID { get; set; }
        public virtual User User { get; set; }
        public virtual Region Region { get; set; }
    }
}