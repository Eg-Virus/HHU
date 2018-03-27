using System.Collections.Generic;

namespace AirObservationSystem.HHU.Core.Model.User
{
    public class SystemRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SystemGroup> SystemGroups { get; set; }

    }
}
