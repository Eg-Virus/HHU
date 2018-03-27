using System;
using System.Collections.Generic;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    /// <summary>
    /// Factory class for CommandPacket object
    /// </summary>
    /// <author>Feras 13 Fab. 10</author>
    public static class CommandPacketFactory
    {
        /// <summary>
        /// Constructs the CommandPacket of the Messages Status Request
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="retryCount">The value of the count header in the packet</param>
        /// <returns>CommandPacket with the Messages Status request</returns>
        /// <author>Abdullah(K) 19 May. 14</author>
        public static CommandPacket GetMessagesStatusRequest(List<string> IDs, int retryCount)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers, new MessagesStatusRequsetBody(IDs));
        }

        /// <summary>
        /// Constructs the CommandPacket of the Login Request
        /// </summary>
        /// <param name="hhuid">HHUId of the user</param>
        /// <param name="username">the user name</param>
        /// <param name="password">the password</param>
        /// <param name="retryCount">The value of the count header in the packet</param>
        /// <param name="loginRequestId"></param>
        /// <returns>CommandPacket with the login request</returns>
        /// <author>Feras 13 Fab. 10</author>
        public static CommandPacket GetLoginRequest(string hhuid, string username, string password, int retryCount, int loginRequestId)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers, new LoginRequestBody(hhuid, username, password, loginRequestId));
        }

        /// <summary/>
        /// /////////////////////////////////////////////////////////////////////////////

        public static CommandPacket GetSynchTimeRequest(string hhuid, int retryCount, int loginRequestId)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers, new SynchTimeRequest(DateTime.Now));

        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary/>
        /// /////////////////////////////////////////////////////////////////////////////

        public static CommandPacket GetSynchTimeRequest(int retryCount)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers, new SynchTimeRequest(DateTime.Now));

        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary/>
        /// /////////////////////////////////////////////////////////////////////////////

        public static CommandPacket GetGpsInProgressRequest(bool gpsState, int retryCount)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers, new GpsInProgressRequest(gpsState));

        }

        ///////////////////////////////////////////////////////////////////////////

        public static CommandPacket GetLocation(double longtitude, double latitude, DateTime dateTime, int retryCount)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers, new LocationBody(longtitude, latitude, dateTime));

        }


        //////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Constructs the CommandPacket of the Login Response
        /// </summary>
        /// <param name="userId">the user name</param>
        /// <param name="retryCount">The value of the count header in the packet</param>
        /// <param name="loginRequestId"></param>
        /// <returns>CommandPacket with the login response</returns>
        /// <author>Feras 13 Fab. 10</author>
        public static CommandPacket GetLoginResponse(int userId, int retryCount, int loginRequestId)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers,
                new LoginResponseBody(userId, loginRequestId));
        }

        /// <summary>
        /// Constructs the CommandPacket of the Text Observation
        /// </summary>
        /// <param name="observerStatus">0:Alert, 1:Notification</param>
        /// <param name="bodyText">The text to be sent</param>
        /// <param name="observerName"></param>
        /// <param name="retryCount">The value of the count header in the packet</param>
        /// <param name="dtObservationTime"></param>
        /// <returns>CommandPacket with the Text Observation</returns>
        /// <author>Feras 13 Fab. 10</author>
        public static CommandPacket GetTextObservation(ObserverStatus observerStatus, string bodyText,
            string observerName, int retryCount, DateTime dtObservationTime)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers,
                new TextObservationBody(observerStatus, bodyText, observerName
                    , dtObservationTime));
        }

        /// <summary>
        /// Constructs the CommandPacket of the Audible Observation
        /// </summary>
        /// <param name="noOfTargets">1:One, 2:Two, 3:Many</param>
        /// <param name="targetType">The type of target</param>
        /// <param name="altitude">1:Low, 2:Mid, 3:High</param>
        /// <param name="heading">The heading of the target</param>
        /// <param name="observerName"></param>
        /// <param name="retryCount">The value of the count header in the packet</param>
        /// <param name="dtObservationTime"></param>
        /// <returns>CommandPacket with the Audible Observation</returns>
        /// <author>Feras 13 Fab. 10</author>
        public static CommandPacket GetAudibleObservation(byte noOfTargets, 
                                                          byte targetType,
                                                          byte altitude, 
                                                          byte heading, 
                                                          string observerName, 
                                                          int retryCount, 
                                                          DateTime dtObservationTime)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers,
                new AudibleObservationBody((NumberofTargets)noOfTargets, 
                                           (TargetType)targetType,
                                           (Altitude)altitude, 
                                           (Heading)heading, 
                                           observerName, 
                                           dtObservationTime));
        }

        /// <summary>
        /// Constructs the CommandPacket of the Visual Observation
        /// </summary>
        /// <param name="noOfTargets">1:One, 2:Two, 3:Many</param>
        /// <param name="targetType">The type of target</param>
        /// <param name="altitude">1:Low, 2:Mid, 3:High</param>
        /// <param name="heading">The heading of the target</param>
        /// <param name="observerName"></param>
        /// <param name="retryCount">The value of the count header in the packet</param>
        /// <param name="dtObservationTime"></param>
        /// <returns>CommandPacket with the Visual Observation</returns>
        /// <author>Feras 13 Fab. 10</author>
        public static CommandPacket GetVisualObservation(byte noOfTargets, byte targetType,
            byte altitude, byte heading, string observerName, int retryCount, DateTime dtObservationTime)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers,
                new VisualObservationBody((NumberofTargets)noOfTargets, 
                                          (TargetType)targetType,
                                          (Altitude)altitude, 
                                          (Heading)heading, 
                                          observerName, 
                                          dtObservationTime));
        }

        /// <summary>
        /// Generate a keep Alive packet that used to ping a server for live communication
        /// </summary>
        /// <returns><see cref="CommandPacket"/> that holds the keep alive packet</returns>
        public static CommandPacket GetKeepAlive()
        {
            return new CommandPacket(Command.Packet, new KeepAliveBody());
        }

        /// <summary>
        /// Constructs the CommandPacket of the Login Response
        /// </summary>
        /// <param name="body"></param>
        /// <param name="retryCount">The value of the count header in the packet</param>
        /// <returns>CommandPacket with the login response</returns>
        /// <author>Feras 13 Fab. 10</author>
        public static CommandPacket GetPacket(IBody body, int retryCount)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers, body);
        }

        //public static CommandPacket GetStatusRequestOld(StatusRequestBody.Status status, int retryCount)
        //{
        //    var headers = new HeaderList();
        //    headers.AddHeader("Count", retryCount.ToString());
        //    return new CommandPacket(Command.Packet, headers, new StatusRequestBody(status));

        //}

        public static CommandPacket GetStatusRequest(RequestPacket status, int retryCount)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers, new StatusRequestBody(status));
        }

        public static CommandPacket GetStatusRequest(RequestPacket status, DateTime T0, int retryCount)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers, new StatusRequestBody(status, T0));
        }

        public static CommandPacket GetCallDestination(DestinationCall dc, int retryCount)
        {
            var headers = new HeaderList();
            headers.AddHeader("Count", retryCount.ToString());
            return new CommandPacket(Command.Packet, headers, new CallDestinationBody(dc));

        }
    }
}
