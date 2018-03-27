using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class LockBody : IBody
    {
        public string HHUId { get; set; }

        public LockBody(string hhuid)
        {
            if (Encoding.UTF8.GetByteCount(hhuid) > (int)byte.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(hhuid), hhuid);
            HHUId = hhuid;
        }

        public LockBody(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                HHUId = reader.ReadString();
            }
        }

        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.Lock;

        public byte[] AsByteArray()
        {
            var bodyArray = new List<byte>();
            var bodyBytes = new List<byte>();

            var body = new byte[2];

            Array.Copy(BitConverter.GetBytes((ushort)TypeFlag), 0, body, 0, 2);

            bodyBytes.AddRange(body);

            bodyBytes.Add((byte)HHUId.Length);
            bodyBytes.AddRange(Encoding.UTF8.GetBytes(HHUId));

            bodyArray.AddRange(BitConverter.GetBytes((ushort)bodyBytes.Count));
            bodyArray.AddRange(bodyBytes.ToArray());

            return bodyArray.ToArray();
        }

        #endregion
    }
}
