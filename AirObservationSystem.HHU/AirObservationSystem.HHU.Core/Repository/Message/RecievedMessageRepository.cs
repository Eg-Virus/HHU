
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.Message;
using AirObservationSystem.HHU.Core.Repository.Base;

namespace AirObservationSystem.HHU.Core.Repository.Message
{
    public class RecievedMessageRepository : RepositoryBase<RecievedMessage>, IRecievedMessageRepository
    {
        public RecievedMessageRepository(IDatabaseFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
    public interface IRecievedMessageRepository : IRepository<RecievedMessage>
    {
    }

}

