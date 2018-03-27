using System.Collections.Generic;
using AirObservationSystem.HHU.Core.Model.Message;

namespace AirObservationSystem.HHU.Core.Model.User
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Rank { get; set; }
        public bool IsLogedIn { get; set; }
        public int LogInMsgId { get; set; }
        public int LoginTimeout { get; set; } //TODO:LoginTimeout

        public virtual ICollection<SystemGroup> SystemGroups { get; set; }
        public virtual ICollection<UserRegion> UserRegions { get; set; }

    }
}
