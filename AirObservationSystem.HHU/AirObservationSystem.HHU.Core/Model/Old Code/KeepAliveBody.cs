using System;
using System.Collections.Generic;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class KeepAliveBody : IBody
    {
        public KeepAliveBody()
        { }

        public KeepAliveBody(byte[] data)
        {
            if (data.Length < 1 || data[0] != magicBytes[0])
                throw new ArgumentOutOfRangeException("Invalid Magic Byte");
        }

        // And dont ask why? chage it if you want...it in your own risk
        private static byte[] magicBytes = new byte[]{0xCC}; 
        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.KeepAlive;

        public byte[] AsByteArray()
        {
            var byteArray = new List<byte>();
            var keepAlive = new List<byte>();

            keepAlive.AddRange(BitConverter.GetBytes((ushort)TypeFlag));
            keepAlive.AddRange(magicBytes);

            byteArray.AddRange(BitConverter.GetBytes((ushort)keepAlive.Count));
            byteArray.AddRange(keepAlive.ToArray());
            return byteArray.ToArray();
        }

        #endregion
    }
}
