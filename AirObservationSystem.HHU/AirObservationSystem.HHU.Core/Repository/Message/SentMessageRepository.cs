using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.Message;
using AirObservationSystem.HHU.Core.Repository.Base;

namespace AirObservationSystem.HHU.Core.Repository.Message
{
    public class SentMessageRepository : RepositoryBase<SentMessage>, ISentMessageRepository
    {
        public SentMessageRepository(IDatabaseFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
    public interface ISentMessageRepository : IRepository<SentMessage>
    {
    }
    
}
