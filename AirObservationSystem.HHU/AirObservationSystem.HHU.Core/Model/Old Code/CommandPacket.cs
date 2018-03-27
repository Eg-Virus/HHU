using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AirObservationSystem.HHU.Core.Helpers;
using AirObservationSystem.HHU.Core.PlatformInterface;
using Xamarin.Forms;
using Command = AirObservationSystem.HHU.Core.Helpers.Command;
using Autofac;
using AirObservationSystem.HHU.Core.Infrastructure;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    /// <summary>
    /// Used to construct full packet the consists of Command type, headers, and body
    /// </summary>
    /// <author>Feras 13 Fab. 10</author>
    public class CommandPacket
    {
        /// <summary>
        /// The command type of the packet
        /// </summary>
        /// <author>Feras 13 Fab. 10</author>
        public Command Command { get; }

        /// <summary>
        /// The headers of the packet
        /// </summary>
        /// <author>Feras 13 Fab. 10</author>
        public HeaderList Headers { get; set; }

        /// <summary>
        /// The body of the packet
        /// </summary>
        /// <author>Feras 13 Fab. 10</author>
        public IBody Body { get; set; }

        public CommandPacket(Command command, HeaderList headers, IBody body)
        {
            if (Enum.IsDefined(typeof(Command), command))
                Command = command;
            else
                throw new ArgumentOutOfRangeException(nameof(command), "Invalid Enamuration Value");

            Headers = headers;
            Body = body;
        }

        public CommandPacket(Command command, IBody body)
        {
            Command = command;
            Headers = new HeaderList();
            Body = body;
        }

        public CommandPacket(Command command)
        {
            Command = command;
            Headers = new HeaderList();
        }

        /// <summary>
        /// Constructor that takes byte array and converts it to Command Packet
        /// </summary>
        /// <param name="data">The byte array to be converted</param>
        /// <author>Feras 13 Fab. 10</author>
        public CommandPacket(byte[] data)
        {
            var packetSize = 0;
            try
            {
                packetSize = BitConverter.ToInt16(data.Take(2).ToArray(), 0);
            }
            catch (Exception)
            {
                throw new InvalidDataException("The packet is not on the right format");
            }
            if (packetSize != data.Length)
                throw new InvalidDataException("The packet is not on the right format");

            Command = (Command)data[2];

            Headers = new HeaderList(data.Skip(3).ToArray());
            var size = Headers.AsByteArray().Length + 3;

            var bodyArray = data.Skip(size).ToArray();

            using (var reader = new BinaryReader(new MemoryStream(bodyArray)))
            {
                int bodysize = reader.ReadInt16();

                if (bodysize == (bodyArray.Length - 6) && bodysize > 0)
                {
                    var flag = (BodyFlag) reader.ReadInt16();
                    var bodyBytes = reader.ReadBytes(bodysize - 2);

                    #region BodyFlag
                    switch (flag)
                    {
                        case BodyFlag.LoginResponse:
                            Body = new LoginResponseBody(bodyBytes);
                            break;
                        case BodyFlag.TextObservation:
                            Body = new TextObservationBody(bodyBytes);
                            break;
                        case BodyFlag.AudibleObservation:
                            Body = new AudibleObservationBody(bodyBytes);
                            break;
                        case BodyFlag.VisualObservation:
                            Body = new VisualObservationBody(bodyBytes);
                            break;
                        case BodyFlag.GsmConnNotification:
                            Body = new GSMConnectNotificationBody(bodyBytes);
                            break;
                        case BodyFlag.SynchTimeResponse:
                            Body = new SynchTimeResponse(bodyBytes);
                            break;
                        case BodyFlag.Lock:
                            Body = new LockBody(bodyBytes);
                            break;
                        case BodyFlag.Destroy:
                            Body = new DestroyBody();
                            break;
                        case BodyFlag.ClientMsg:
                            Body = new ClientMsgBody(bodyBytes);
                            break;
                        case BodyFlag.PhoneCall:
                            Body = new PhoneCallBody(bodyBytes);
                            break;
                        case BodyFlag.PhoneCallMissed:
                            Body = new MessagesStatusResponseBody(bodyBytes);
                            break;
                        case BodyFlag.GsmSmsResult:
                            Body = new GsmMsgSendResultBody(bodyBytes);
                            break;
                        case BodyFlag.KeepAlive:
                            Body = new KeepAliveBody(bodyBytes);
                            break;
                        case BodyFlag.Location:
                            Body = new LocationBody(bodyBytes);
                            break;
                        case BodyFlag.MessagesStatusResponse:
                            Body = new MessagesStatusResponseBody(bodyBytes);
                            break;
                        case BodyFlag.CallDestination:
                            Body = new CallDestinationBody(bodyBytes);
                            break;
                        case BodyFlag.PlatoonMsg:
                            Body = new PlatoonMsgBody(bodyBytes);
                            break;
                        case BodyFlag.AvailableCallingMethod:
                            Body = new AvailableCallingMethodBody(bodyBytes);
                            break;
                        case BodyFlag.None:
                            break;
                        case BodyFlag.LoginRequest:
                            break;
                        case BodyFlag.Deauthenticate:
                            break;
                        case BodyFlag.ApplyFailoverMsg:
                            break;
                        case BodyFlag.FailoverResponseMsg:
                            break;
                        case BodyFlag.MsgAcknowledged:
                            break;
                        case BodyFlag.SynchTimeRequest:
                            break;
                        case BodyFlag.GpsInProgress:
                            break;
                        case BodyFlag.MessagesStatusRequest:
                            break;
                        case BodyFlag.Status:
                            break;
                        case BodyFlag.MsgReceived:
                            break;
                        default:
                            Static.Log("AirObservationSystem.HHU.Core.Model.Old_Code.CommandPacket.CommandPacket", LogType.Error, "The body format is invalid");
                            break;
                    } 
                    #endregion

                    if (Body == null)
                        Static.Log("AirObservationSystem.HHU.Core.Model.Old_Code.CommandPacket.CommandPacket", LogType.Information,
                            "-Convert byte[] to CommandPacket: \n" + Headers.ToString());
                    else
                        Static.Log("AirObservationSystem.HHU.Core.Model.Old_Code.CommandPacket.CommandPacket", LogType.Information,
                            "-Convert byte[] to CommandPacket: \n" + Headers.ToString() + Body.ToString());
                }
                //Acknowledgement messages has zero body size, no need to give a log, it will happen latter
                else if (Command == Command.Acknowledgement)
                {
                }

                else if (bodysize == 0)
                    Static.Log("AirObservationSystem.HHU.Core.Model.Old_Code.CommandPacket.CommandPacket", LogType.Information, "Body Size is Zero");
                else
                    Static.Log("AirObservationSystem.HHU.Core.Model.Old_Code.CommandPacket.CommandPacket", LogType.Information, "Body Size is Invalid");

                var crc32 = reader.ReadBytes(4);

                if (!CheckCrc32(data.Take(data.Count() - 4).ToArray(), crc32))
                    Command = Command.CrcError;

            }

        }

        /// <summary>
        /// Converts the packet to byte array
        /// </summary>
        /// <returns>packet byte array</returns>
        /// <author>Feras 13 Fab. 10</author>
        public byte[] AsByteArray()
        {
            var headersBytes = Headers.AsByteArray();
            byte[] bodyBytes = null;
            ushort totalSize;

            if (Body != null)
            {
                bodyBytes = Body.AsByteArray();
                totalSize = Convert.ToUInt16(headersBytes.Length + bodyBytes.Length + 7);
            }
            else
                totalSize = Convert.ToUInt16(headersBytes.Length + 9);
            var packetArray = new List<byte>();
            packetArray.AddRange(BitConverter.GetBytes(totalSize));
            packetArray.Add((byte)Command);

            packetArray.AddRange(headersBytes);
            if (Body != null)
                packetArray.AddRange(bodyBytes);
            else
                packetArray.AddRange(BitConverter.GetBytes(Convert.ToInt16(0)));
            var alg = new Algorithm("CRC32", AppContainer.Container.Resolve<ICrc32>());
            alg.Hash.Initialize();

            var hash = alg.Hash.ComputeHash(packetArray.ToArray());

            packetArray.AddRange(hash);

            return packetArray.ToArray();
        }

        /// <summary>
        /// Check if the CRC is correct or not
        /// </summary>
        /// <param name="message">The full message in byte array</param>
        /// <param name="crc">the CRC of the message in byte array</param>
        /// <returns>true if CRC is correct, false otherwise</returns>
        /// <author>Feras 13 Fab. 10</author>
        /// <remarks>the CRC of the message and the crc byte array must have the same count</remarks>
        private bool CheckCrc32(byte[] message, byte[] crc)
        {
            
            var alg = new Algorithm("CRC32", AppContainer.Container.Resolve<ICrc32>());
            alg.Hash.Initialize();

            var hash = alg.Hash.ComputeHash(message);

            if (hash.Count() == crc.Count())
            {
                for (var i = 0; i < hash.Count(); i++)
                {
                    if (hash[i] != crc[i])
                        return false;
                }
                return true;
            }

            return false;
        }

    }
}
