using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Repository.Base;

namespace AirObservationSystem.HHU.Core.Repository.User
{
    //[ExcludeFromCodeCoverage]

    public class UserRepository : RepositoryBase<Model.User.User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory dbFactory)
            : base(dbFactory)
        {
        }

    }
    public interface IUserRepository : IRepository<Model.User.User>
    {

    }
}
