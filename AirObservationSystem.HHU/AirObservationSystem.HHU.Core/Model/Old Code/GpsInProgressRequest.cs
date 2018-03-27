using System;
using System.Collections.Generic;
using System.IO;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class GpsInProgressRequest : IBody
    {
        public GpsInProgressRequest(bool state)
        {
            GpsState = state;
        }

        public GpsInProgressRequest(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                GpsState = reader.ReadByte() == 1;
            }
        }

        public bool GpsState
        { get; }




        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.GpsInProgress;

        public byte[] AsByteArray()
        {
            var byteArray = new List<byte>();
            var data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((ushort)TypeFlag));
            data.AddRange(BitConverter.GetBytes(GpsState));

            byteArray.AddRange(BitConverter.GetBytes((ushort)byteArray.Count));
            byteArray.AddRange(data);

            return byteArray.ToArray();
        }

        #endregion

    }
}
