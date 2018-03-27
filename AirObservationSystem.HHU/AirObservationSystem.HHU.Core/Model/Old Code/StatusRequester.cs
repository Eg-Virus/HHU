using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    /// <summary>
    /// a Factory class that generate a specific request patterns
    /// </summary>
    public class StatusRequester
    {
        /// <summary>
        /// initial empty packet
        /// </summary>
        /// <returns></returns>
        public static RequestPacket InitialPacket()
        {
            return new RequestPacket();
        }

        /// <summary>
        /// initial packet containing all the subtypes of the request intended 
        /// </summary>
        /// <returns></returns>
        public static RequestPacket LoginRequest()
        {
            return new RequestPacket()
                .StatusRequest(RequestPacketTypes.AvailableCallingMethod)
                .StatusRequest(RequestPacketTypes.Messages)
                .StatusRequest(RequestPacketTypes.PlatoonMsgs)
                .StatusRequest(RequestPacketTypes.MissedCalls);
        }

        /// <summary>
        /// initial packet containing all the subtypes of the request intended 
        /// </summary>
        /// <returns></returns>
        public static RequestPacket ConnectionStateChange()
        {
            return new RequestPacket()
                .StatusRequest(RequestPacketTypes.GSM)
                .StatusRequest(RequestPacketTypes.Messages)
                .StatusRequest(RequestPacketTypes.PlatoonMsgs)
                .StatusRequest(RequestPacketTypes.MissedCalls)
                .StatusRequest(RequestPacketTypes.AvailableCallingMethod);
        }

        /// <summary>
        /// initial packet containing one request
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static RequestPacket SingleStatusRequest(RequestPacketTypes types)
        {
            return new RequestPacket(types);
        }

        public static RequestPacket DecodePacket(uint raw)
        {
            return new RequestPacket(raw);
        }

        public static RequestPacket DebugLoginRequest()
        {
            return new RequestPacket()
                .StatusRequest(RequestPacketTypes.AvailableCallingMethod)
                .StatusRequest(RequestPacketTypes.Messages)
                .StatusRequest(RequestPacketTypes.PlatoonMsgs);
        }

        public static RequestPacket StatusMessagesRequest()
        {
            return new RequestPacket()
                .StatusRequest(RequestPacketTypes.MissedCalls)
                .StatusRequest(RequestPacketTypes.Messages)
                .StatusRequest(RequestPacketTypes.PlatoonMsgs);
        }
    }
}
