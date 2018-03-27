using System;
using System.Collections.Generic;
using System.IO;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    /// <summary>
    /// Identify The Destination call
    /// </summary>
    

    public class CallDestinationBody : IBody
    {
        public DestinationCall DestCall { get; }


        public CallDestinationBody(DestinationCall dc)
        {
            DestCall = dc;
        }

        public CallDestinationBody(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                DestCall = (DestinationCall) reader.ReadByte();
            }
        }

        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.CallDestination;

        public byte[] AsByteArray()
        {
            var bodyArray = new List<byte>();
            var bodyBytes = new List<byte>();

            bodyBytes.AddRange(BitConverter.GetBytes((ushort)TypeFlag));
            bodyBytes.Add((byte)DestCall);

            bodyArray.AddRange(BitConverter.GetBytes((ushort)bodyBytes.Count));
            bodyArray.AddRange(bodyBytes.ToArray());

            return bodyArray.ToArray();
        }

        #endregion
    }
}
