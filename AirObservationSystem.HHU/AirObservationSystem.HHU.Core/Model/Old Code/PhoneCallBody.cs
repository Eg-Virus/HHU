using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public enum CallStatus : byte { Ring = 0, Started, Ended, Missed }
    public class PhoneCallBody : IBody
    {
        public CallStatus Status { get; set; }

        public string CallerNumber { get; set; } = "";

        public PhoneCallBody(CallStatus status)
        {
            if (Enum.IsDefined(typeof(CallStatus), status))
                Status = status;
            else
                throw new ArgumentOutOfRangeException(nameof(status), "Invalid Enamuration Value");
        }
        public PhoneCallBody(CallStatus status, string caller)
        {
            if (Enum.IsDefined(typeof(CallStatus), status))
                Status = status;
            else
                throw new ArgumentOutOfRangeException(nameof(status), "Invalid Enamuration Value");
            CallerNumber = caller;
        }
        public PhoneCallBody(byte[] data)
        {
            var reader = new BinaryReader(new MemoryStream(data));
            Status = (CallStatus)reader.ReadByte();

            var size = (int)reader.ReadInt32();
            CallerNumber = Encoding.UTF8.GetString(reader.ReadBytes(size), 0, size);
        }

        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.PhoneCall;

        public byte[] AsByteArray()
        {
            var byteArray = new List<byte>();
            var callStatus = new List<byte>();

            callStatus.AddRange(BitConverter.GetBytes((ushort)TypeFlag));
            callStatus.Add((byte)Status);

            var b = Encoding.UTF8.GetBytes(CallerNumber);
            callStatus.AddRange(BitConverter.GetBytes(b.Length));
            callStatus.AddRange(b);

            byteArray.AddRange(BitConverter.GetBytes((ushort)callStatus.Count));
            byteArray.AddRange(callStatus.ToArray());

            return byteArray.ToArray();
        }
        #endregion
    }
}
