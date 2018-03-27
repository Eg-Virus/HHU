using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.User;
using AirObservationSystem.HHU.Core.Repository.Base;

namespace AirObservationSystem.HHU.Core.Repository.User
{
    public class SystemGroupRepository : RepositoryBase<SystemGroup>, ISystemGroupRepository
    {
        public SystemGroupRepository(IDatabaseFactory dbFactory)
            : base(dbFactory)
        {
        }

    }

    public interface ISystemGroupRepository : IRepository<SystemGroup>
    {

    }
}