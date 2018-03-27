using System;
using System.Collections.Generic;
using System.Text;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    /// <summary>
    /// Request Packet which contain all request up to 32 concurrent request in one unsigned integer
    /// </summary>
    public class RequestPacket
    {
        /// <summary>
        /// Enum Containing all bit patterns for the Request to have
        /// </summary>
        

        /// <summary>
        /// Raw data stored into this variable
        /// </summary>
        private uint _requestBitPattern;

        /// <summary>
        /// SyncTime Request, Data
        /// </summary>
        public DateTime T0 { get; private set; } // for the syncTime Request

        /// <summary>
        /// constructor to decode a raw uint data, extracted from a received buffer
        /// </summary>
        /// <param name="raw"></param>
        public RequestPacket(uint raw)
        {
            _requestBitPattern = raw;
            T0 = DateTime.MinValue;
        }

        /// <summary>
        /// constructor to decode a raw uint data, extracted from a received buffer along with the TimeSync T0 Value
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="t0"></param>
        public RequestPacket(uint raw, DateTime t0)
        {
            _requestBitPattern = raw;
            T0 = t0;
        }

        /// <summary>
        /// a constructor to create an initial empty request
        /// </summary>
        public RequestPacket()
        {
            _requestBitPattern = (uint)RequestPacketTypes.Initial;
        }

        /// <summary>
        /// a constructor that create a request packet with on request
        /// </summary>
        /// <param name="types"></param>
        public RequestPacket(RequestPacketTypes types)
        {
            _requestBitPattern = (uint)types;
        }

        /// <summary>
        /// function to be used for testing the request whether it was empty or NOT
        /// </summary>
        /// <returns></returns>
        public bool IsEmptyRequest()
        {
            return _requestBitPattern == (uint)RequestPacketTypes.Initial;
        }

        /// <summary>
        /// a function that test if a buffer contains that specified request
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Contains(RequestPacketTypes type)
        {
            var pattern = _requestBitPattern;
            return (pattern & (uint)type) > 0;
        }

        /// <summary>
        /// function is used to add the request into the bitpattern to be used before sending the request
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public RequestPacket StatusRequest(RequestPacketTypes types)
        {
            if (types == RequestPacketTypes.Initial)
            {
                _requestBitPattern = (uint)RequestPacketTypes.Initial;
            }
            else if (types == RequestPacketTypes.SyncTime)
            {
                throw new Exception("StatusRequestSyncTime function should be used along the side with SyncTime request");
            }
            else
            {
                _requestBitPattern |= (uint)types;
            }
            return this;
        }

        public RequestPacket StatusRequestSyncTime(DateTime t0)
        {
            _requestBitPattern |= (uint)RequestPacketTypes.SyncTime;
            T0 = t0;

            return this;
        }

        /// <summary>
        /// overrided toString to be used in printing the bit Pattern
        /// </summary>
        /// <returns></returns>
        public string ToBinaryString()
        {
            return Convert.ToString(_requestBitPattern, 2);
        }

        public override string ToString()
        {
            if (IsEmptyRequest())
            {
                return "[[Empty Status Request]]";
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append("[[");
                sb.AppendLine();

                var fields = GetFields(typeof(RequestPacketTypes));
                foreach (var item in fields)
                {
                    sb.Append($"{item.Value}={Contains((RequestPacketTypes) item.Value)}");
                    sb.AppendLine();
                }

                sb.Append("]]");

                return sb.ToString();
            }
        }

        public Dictionary<RequestPacketTypes, bool> ToDict()
        {
            var ret = new Dictionary<RequestPacketTypes, bool>();

            if (!IsEmptyRequest())
            {
                var fields = GetFields(typeof(RequestPacketTypes));
                foreach (var item in fields)
                {
                    ret[(RequestPacketTypes)item.Value] = Contains((RequestPacketTypes)item.Value);
                }
            }
            return ret;
        }

        public static Dictionary<int, object> GetFields(Type type)
        {
            var i = 0;
            var dict = new Dictionary<int, object>();


            var types = Enum.GetValues(type);
            foreach (var item in types)
            {
                dict[i++] = item;
            }

            return dict;
        }

        /// <summary>
        /// the size of the request, always of the type used in the class, uint
        /// </summary>
        public int Size => sizeof(uint) + SizeOfSyncTime;

        private int SizeOfSyncTime => Contains(RequestPacketTypes.SyncTime) ? sizeof(long) : 0;

        /// <summary>
        /// raw uint data that is used in a request message body
        /// </summary>
        public uint RAW => _requestBitPattern;

        /// <summary>
        /// checks if the raw data has a sync request
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static bool ContainsSyncRequest(uint raw)
        {
            return new RequestPacket(raw).Contains(RequestPacketTypes.SyncTime);
        }
    }
}
