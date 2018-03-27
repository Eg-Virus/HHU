using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class PlatoonMsgBody : IBody
    {
        public string HHUID { get; set; }

        public string Text { get; set; }

        public string PlatoonName { get; set; }

        public DateTime SendDate { get; set; }

        public PlatoonMsgBody(string hhuid, string text, string platoonName, DateTime sendDate)
        {
            HHUID = hhuid;
            Text = text;
            PlatoonName = platoonName;
            SendDate = sendDate;
        }

        public PlatoonMsgBody(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                int size;
                size = (int) reader.ReadByte();
                HHUID = Encoding.UTF8.GetString(reader.ReadBytes(size), 0, size);
                size = reader.ReadUInt16();
                Text = Encoding.Unicode.GetString(reader.ReadBytes(size), 0, size);
                size = reader.ReadUInt16();
                PlatoonName = Encoding.Unicode.GetString(reader.ReadBytes(size), 0, size);
                SendDate = new DateTime(reader.ReadInt64());
            }
        }

        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.PlatoonMsg;

        public byte[] AsByteArray()
        {
            var buffer = new List<byte>();
            var array = new List<byte>();

            buffer.AddRange(BitConverter.GetBytes((ushort)TypeFlag));

            var bytesStr = Encoding.UTF8.GetBytes(HHUID);
            buffer.Add((byte)bytesStr.Length);
            buffer.AddRange(bytesStr);

            bytesStr = Encoding.Unicode.GetBytes(Text);
            buffer.AddRange(BitConverter.GetBytes((ushort)bytesStr.Length));
            buffer.AddRange(bytesStr);

            bytesStr = Encoding.Unicode.GetBytes(PlatoonName);
            buffer.AddRange(BitConverter.GetBytes((ushort)bytesStr.Length));
            buffer.AddRange(bytesStr);

            buffer.AddRange(BitConverter.GetBytes(SendDate.Ticks));

            array.AddRange(BitConverter.GetBytes((ushort)buffer.Count));
            array.AddRange(buffer.ToArray());
            return array.ToArray();
        }

        #endregion
    }
}
