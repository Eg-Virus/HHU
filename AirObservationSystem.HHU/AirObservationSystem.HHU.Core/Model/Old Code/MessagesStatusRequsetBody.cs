using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class MessagesStatusRequsetBody : IBody
    {
        public MessagesStatusRequsetBody(List<string> ids)
        {
            IDs = ids;
        }

        public MessagesStatusRequsetBody(byte[] data)
        {
            IDs = new List<string>();
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                var count = reader.ReadInt32();

                for (var i = 0; i < count; i++)
                {
                    var bytesToDecode = reader.ReadInt32();
                    IDs.Add(Encoding.UTF8.GetString(reader.ReadBytes(bytesToDecode), 0, bytesToDecode));
                }
            }
        }



        #region IBody Members

        public List<string> IDs { get; }

        public BodyFlag TypeFlag => BodyFlag.MessagesStatusRequest;

        public byte[] AsByteArray()
        {
            var bodyArray = new List<byte>();
            var body = new List<byte>();

            body.AddRange(BitConverter.GetBytes((ushort)this.TypeFlag));
            body.AddRange(BitConverter.GetBytes(IDs.Count));

            foreach (var id in IDs)
            {
                var idBytes = Encoding.UTF8.GetBytes(id);
                body.AddRange(BitConverter.GetBytes(idBytes.Length));
                body.AddRange(idBytes);
            }

            bodyArray.AddRange(BitConverter.GetBytes((ushort)body.Count));
            bodyArray.AddRange(body.ToArray());

            return bodyArray.ToArray();
        }

        #endregion

    }
}
