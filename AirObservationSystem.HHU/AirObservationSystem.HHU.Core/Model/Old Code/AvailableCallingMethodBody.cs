using System;
using System.Collections.Generic;
using System.IO;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    /// <summary>
    /// Identify The Available Calling Method call
    /// </summary>
   

    public class AvailableCallingMethodBody : IBody
    {
        public AvailableCallingMethod AvaCall { get; }


        public AvailableCallingMethodBody(AvailableCallingMethod ava)
        {
            AvaCall = ava;
        }

        public AvailableCallingMethodBody(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                AvaCall = (AvailableCallingMethod) reader.ReadByte();
            }
        }

        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.AvailableCallingMethod;

        public byte[] AsByteArray()
        {
            var bodyArray = new List<byte>();
            var bodyBytes = new List<byte>();

            bodyBytes.AddRange(BitConverter.GetBytes((ushort)TypeFlag));
            bodyBytes.Add((byte)AvaCall);

            bodyArray.AddRange(BitConverter.GetBytes((ushort)bodyBytes.Count));
            bodyArray.AddRange(bodyBytes.ToArray());

            return bodyArray.ToArray();
        }

        #endregion
    }
}
