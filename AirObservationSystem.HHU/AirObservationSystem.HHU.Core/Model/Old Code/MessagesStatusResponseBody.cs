using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class MessagesStatusResponseBody : IBody
    {
        public MsgType MsgStausType;
        public MessagesStatusResponseBody(List<MessagesStatus> messages, MsgType type)
        {
            MsgStausType = type;
            Messages = messages;
        }

        public MessagesStatusResponseBody(byte[] data)
        {
            Messages = new List<MessagesStatus>();
            var reader = new BinaryReader(new MemoryStream(data));
            var count = reader.ReadInt32();
            MsgStausType = (MsgType)reader.ReadInt32();

            if (MsgStausType == MsgType.MissedCall || MsgStausType == MsgType.MissedSMS)
            {
                for (var i = 0; i < count; i++)
                {
                    var callerSize = reader.ReadInt32();
                    #region explain comments
                    //Sowayeh, Bander wrote this part:
                    //data could be caller name in missedCall state.
                    //      and could be sms contents in missed SMS state
                    var data1 = Encoding.UTF8.GetString(reader.ReadBytes(callerSize), 0, callerSize);
                    var calltime = new DateTime(reader.ReadInt64());
                    var t = (MsgType)reader.Read();
                    #endregion
                    var msg = new MessagesStatus(t, data1, calltime);
                    Messages.Add(msg);
                }
            }
            else if (MsgStausType == MsgType.PlatoonMsg)
            {
                for (var i = 0; i < count; i++)
                {
                    var contentSize = reader.ReadInt32();
                    var content = Encoding.UTF8.GetString(reader.ReadBytes(contentSize), 0, contentSize);
                    var sourceSize = reader.ReadInt32();
                    var source = Encoding.UTF8.GetString(reader.ReadBytes(sourceSize), 0, sourceSize);
                    var time = new DateTime(reader.ReadInt64());
                    var type = (MsgType)reader.Read();

                    var msg = new MessagesStatus(content, source, time);
                    Messages.Add(msg);
                }
            }
            else
            {
                for (var i = 0; i < count; i++)
                {
                    var bytesToDecode = reader.ReadInt32();
                    var msg = new MessagesStatus(Encoding.UTF8.GetString(reader.ReadBytes(bytesToDecode), 0, bytesToDecode), (GsmMsgSendResult)reader.ReadByte());
                    Messages.Add(msg);
                }
            }
        }

        #region IBody Members

        public List<MessagesStatus> Messages { get; }

        public BodyFlag TypeFlag => BodyFlag.MessagesStatusResponse;

        public byte[] AsByteArray()
        {
            var bodyArray = new List<byte>();
            var body = new List<byte>();

            body.AddRange(BitConverter.GetBytes((ushort)TypeFlag));
            body.AddRange(BitConverter.GetBytes(Messages.Count));
            body.AddRange(BitConverter.GetBytes((int)MsgStausType));

            foreach (var msg in Messages)
            {
                if (msg.Type == MsgType.MissedCall)
                {
                    //Caller#
                    var bytesStr = Encoding.UTF8.GetBytes(msg.SourceNumber);
                    body.AddRange(BitConverter.GetBytes(bytesStr.Length));
                    body.AddRange(bytesStr);

                    //Call Time
                    body.AddRange(BitConverter.GetBytes(msg.NotificationTime.Ticks));
                    // message status type
                    body.Add((byte)MsgType.MissedCall);
                }
                else if (msg.Type == MsgType.MissedSMS)
                {
                    //Message Contents
                    var bytesStr = Encoding.UTF8.GetBytes(msg.SmsContent);
                    body.AddRange(BitConverter.GetBytes(bytesStr.Length));
                    body.AddRange(bytesStr);

                    //SMS Time
                    body.AddRange(BitConverter.GetBytes(msg.NotificationTime.Ticks));
                    // message status type
                    body.Add((byte)MsgType.MissedSMS);
                }
                else if (msg.Type == MsgType.PlatoonMsg)
                {
                    //Message Contents
                    var bytesStr = Encoding.UTF8.GetBytes(msg.SmsContent);
                    body.AddRange(BitConverter.GetBytes(bytesStr.Length));
                    body.AddRange(bytesStr);
                    //Source
                    var bytesSour = Encoding.UTF8.GetBytes(msg.SourceNumber);
                    body.AddRange(BitConverter.GetBytes(bytesSour.Length));
                    body.AddRange(bytesSour);
                    //SMS Time
                    body.AddRange(BitConverter.GetBytes(msg.NotificationTime.Ticks));
                    //message status type
                    body.Add((byte)MsgType.PlatoonMsg);
                }
                else
                {
                    var idBytes = Encoding.UTF8.GetBytes(msg.Id);
                    body.AddRange(BitConverter.GetBytes(idBytes.Length));
                    body.AddRange(idBytes);
                    body.Add((byte)msg.Status);
                }
            }

            bodyArray.AddRange(BitConverter.GetBytes((ushort)body.Count));
            bodyArray.AddRange(body.ToArray());

            return bodyArray.ToArray();
        }

        #endregion

    }
}
