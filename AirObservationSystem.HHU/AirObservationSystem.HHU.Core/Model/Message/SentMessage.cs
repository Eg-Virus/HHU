using AirObservationSystem.HHU.Core.Helpers;
namespace AirObservationSystem.HHU.Core.Model.Message
{
    public class SentMessage : AMessage
    {
        public MessageStatus Status { get; set; }
        //public MessagesStatus LastStatus { get; set; }
    }
}
