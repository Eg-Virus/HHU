using System.Collections.Generic;

namespace AirObservationSystem.HHU.Core.Model.User
{
    public class SystemGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SystemRole> SystemRoles { get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
}