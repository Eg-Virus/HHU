using System;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class GsmMsgSendResultBody : IBody
    {
        public GsmMsgSendResultBody(long msgID, GsmMsgSendResult gsmSendResult)
        {
            this.msgID = msgID;

            if (Enum.IsDefined(typeof(GsmMsgSendResult), gsmSendResult))
                this.gsmSendResult = gsmSendResult;
            else
                throw new ArgumentOutOfRangeException("gsmSendResult", "Invalid Enamuration Value");
        }
        public GsmMsgSendResultBody(byte[] data)
        {
            msgID = BitConverter.ToInt64(data, 0);
            gsmSendResult = (GsmMsgSendResult)data[8];
        }

        private GsmMsgSendResult gsmSendResult;
        public GsmMsgSendResult GsmSendResult
        {
            get
            {
                return gsmSendResult;
            }
        }

        private long msgID;
        public long MsgID { get { return msgID; } }

        #region IBody Members

        public BodyFlag TypeFlag
        {
            get { return BodyFlag.GsmSmsResult; }
        }

        public byte[] AsByteArray()
        {
            const ushort totalSize =
                sizeof(long) + // MsgID Size
                1 + // GsmMsgSendResult Size
                sizeof(ushort) + // TotalSize Size 
                sizeof(ushort); // BodyFlag Size

            byte[] bodyBytes = new byte[totalSize];

            Buffer.BlockCopy(BitConverter.GetBytes((ushort)totalSize - sizeof(ushort)), 0, bodyBytes, 0, 2);

            Buffer.BlockCopy(BitConverter.GetBytes((ushort)TypeFlag), 0, bodyBytes, 2, 2);

            Buffer.BlockCopy(BitConverter.GetBytes(msgID), 0, bodyBytes, sizeof(ushort) + sizeof(ushort), sizeof(long));

            bodyBytes[sizeof(ushort) + sizeof(ushort) + sizeof(long)] = (byte)gsmSendResult;
            return bodyBytes;
        }

        #endregion
    }
}
