using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.User;
using AirObservationSystem.HHU.Core.Repository.Base;

namespace AirObservationSystem.HHU.Core.Repository.User
{
    public class IUInfoRepository : RepositoryBase<IUInfo>, IIUInfoRepository
    {
        public IUInfoRepository(IDatabaseFactory dbFactory)
            : base(dbFactory)
        {
        }

    }
    public interface IIUInfoRepository : IRepository<IUInfo>
    {

    }
}
