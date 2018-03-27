using System.Collections.Generic;

namespace AirObservationSystem.HHU.Core.Model.User
{
    public class Region
    {

        public int RegionID { get; set; }
        public string Tag { get; set; }
        public string Code { get; set; }
        public string DataCenter { get; set; }

        public virtual ICollection<UserRegion> UserRegions { get; set; }
        public virtual ICollection<IUInfo> IuInfos{ get; set; }

    }
}