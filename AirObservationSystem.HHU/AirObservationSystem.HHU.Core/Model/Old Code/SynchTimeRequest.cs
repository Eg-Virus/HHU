using System;
using System.Collections.Generic;
using System.IO;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class SynchTimeRequest : IBody
    {
        public SynchTimeRequest(DateTime t0)
        {
            T0 = t0;
        }

        public SynchTimeRequest(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                T0 = new DateTime(reader.ReadInt64());
            }
        }

        public DateTime T0 { get; private set; }

        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.SynchTimeRequest;

        public byte[] AsByteArray()
        {
            var byteArray = new List<byte>();
            var synchTimeReq = new List<byte>();

            synchTimeReq.AddRange(BitConverter.GetBytes((ushort) TypeFlag));
            synchTimeReq.AddRange(BitConverter.GetBytes(T0.Ticks));

            byteArray.AddRange(BitConverter.GetBytes((ushort) synchTimeReq.Count));
            byteArray.AddRange(synchTimeReq);

            return byteArray.ToArray();
        }

        #endregion
    }
}
