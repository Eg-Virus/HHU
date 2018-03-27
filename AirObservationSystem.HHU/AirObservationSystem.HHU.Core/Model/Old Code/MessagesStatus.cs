using System;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class MessagesStatus
    {
        public string Id;
        public GsmMsgSendResult Status;
        public DateTime? SentSince;
        public bool IsGuidanceMsg;

        public MsgType Type;
        public string SourceNumber;
        public DateTime NotificationTime;

        public string SmsContent;
        /// <summary>
        /// For Observation Messages, the reset of the parameters will be filled after creation.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public MessagesStatus(string id, GsmMsgSendResult status)
        {
            Id = id;
            Status = status;
            SentSince = DateTime.Now;
            IsGuidanceMsg = false;
            Type = MsgType.HHUMsg;
        }

        //public MessagesStatus(string id, GsmMsgSendResult status, DateTime sentSince, bool isGuidanceMsg)
        //{
        //    this.ID = id;
        //    this.Status = status;
        //    this.SentSince = sentSince;
        //    this.IsGuidanceMsg = isGuidanceMsg;
        //}
        //this constructor used for missed call and SMS
        public MessagesStatus(MsgType msgType, string data, DateTime time)
        {
            SentSince = DateTime.Now;
            Type = msgType;
            //in call we don't need to msg content
            if (msgType == MsgType.MissedSMS)
                SmsContent = data;
            //in sms we don't need to caller number
            else if (msgType == MsgType.MissedCall)
                SourceNumber = data;
            NotificationTime = time;
        }

        /// <summary>
        /// this constructor used for platoon message useually message type is platoonMsg
        /// </summary>
        /// <param name="data">sms content</param>
        /// <param name="source">from any platoon</param>
        /// <param name="time">notification time</param>
        public MessagesStatus(string data, string source, DateTime time)
        {
            SentSince = DateTime.Now;
            Type = MsgType.PlatoonMsg;
            SmsContent = data;
            NotificationTime = time;
            SourceNumber = source;
        }
    }
}