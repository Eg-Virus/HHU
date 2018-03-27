using System;
using System.Collections.Generic;
using System.IO;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class StatusRequestBody : IBody
    {
        private RequestPacket _statusPacket;

        public StatusRequestBody(RequestPacket statusPacket)
        {
            _statusPacket = statusPacket;
        }

        public StatusRequestBody(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                uint statusRaw = reader.ReadUInt32();

                if (RequestPacket.ContainsSyncRequest(statusRaw))
                {
                    var t0 = new DateTime(reader.ReadInt64());
                    _statusPacket = new RequestPacket(statusRaw, t0);
                }
                else
                {
                    _statusPacket = new RequestPacket(statusRaw);
                }

            }
        }

        public StatusRequestBody(RequestPacket statusPacket, DateTime t0)
        {
            _statusPacket = statusPacket;
            T0 = t0;
        }

        public RequestPacket GetStatus()
        {
            return _statusPacket;
        }

        public void SetStatus(RequestPacket status)
        {
            _statusPacket = status;
        }

        public BodyFlag TypeFlag => BodyFlag.Status;

        public DateTime T0 { get; set; }

        public byte[] AsByteArray()
        {
            var bodyArray = new List<byte>();
            var bodyBytes = new List<byte>();

            bodyBytes.AddRange(BitConverter.GetBytes((ushort)TypeFlag));       // 2 bytes
            bodyBytes.AddRange(BitConverter.GetBytes(_statusPacket.RAW));           // 4 bytes

            if (_statusPacket.Contains(RequestPacketTypes.SyncTime))
            {
                bodyBytes.AddRange(BitConverter.GetBytes(_statusPacket.T0.Ticks));  // 8 bytes
            }


            bodyArray.AddRange(BitConverter.GetBytes((ushort)bodyBytes.Count));
            bodyArray.AddRange(bodyBytes.ToArray());

            return bodyArray.ToArray();

        }
    }
}
