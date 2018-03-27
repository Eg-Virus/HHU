using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.User;
using AirObservationSystem.HHU.Core.Repository.Base;

namespace AirObservationSystem.HHU.Core.Repository.User
{
    //[ExcludeFromCodeCoverage]

    public class UserRegionRepository : RepositoryBase<UserRegion>, IUserRegionRepository
    {
        public UserRegionRepository(IDatabaseFactory dbFactory)
            : base(dbFactory)
        {
        }

    }
    public interface IUserRegionRepository : IRepository<UserRegion>
    {

    }
}
