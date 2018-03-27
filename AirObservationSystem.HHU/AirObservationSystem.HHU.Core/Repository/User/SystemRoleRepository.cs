using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.User;
using AirObservationSystem.HHU.Core.Repository.Base;

namespace AirObservationSystem.HHU.Core.Repository.User
{
    //[ExcludeFromCodeCoverage]

    public class SystemRoleRepository : RepositoryBase<SystemRole>, ISystemRoleRepository
    {
        public SystemRoleRepository(IDatabaseFactory dbFactory)
            : base(dbFactory)
        {
        }

    }
    public interface ISystemRoleRepository : IRepository<SystemRole>
    {

    }
}
