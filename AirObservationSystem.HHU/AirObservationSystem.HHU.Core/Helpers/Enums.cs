using System.ComponentModel;

namespace AirObservationSystem.HHU.Core.Helpers
{
    public enum Result
    {
        [Description("Fail")]
        Fail = 0,
        [Description("Success")]
        Success = 1,
    }

    public enum ObservationType
    {
        [Description("Text")]
        Text = 0,
        [Description("Audio")]
        Audio = 1,
        [Description("Video")]
        Video = 2,
    }

    //public enum ObservationNumber
    //{
    //    [Description("Na")]
    //    Na = 0,
    //    [Description("One")]
    //    One = 1,
    //    [Description("Two")]
    //    Two = 2,
    //    [Description("Group")]
    //    Group = 3,
    //}

    //public enum AirborneType
    //{
    //    [Description("Na")]
    //    Na,
    //    [Description("Fighter")]
    //    Fighter,
    //    [Description("Helicopter")]
    //    Helicopter,
    //    [Description("Drone")]
    //    Drone,
    //    [Description("Unidentified")]
    //    Unidentified,
    //    [Description("Civilian")]
    //    Civilian,
    //    [Description("Military")]
    //    Military
    //}

    //public enum Altitude
    //{
    //    [Description("Na")]
    //    Na,
    //    [Description("Low")]
    //    Low,
    //    [Description("Mid")]
    //    Mid,
    //    [Description("High")]
    //    High,
    //}

    //public enum Heading
    //{
    //    [Description("Na")]
    //    Na,
    //    [Description("North")]
    //    North,
    //    [Description("East")]
    //    East,
    //    [Description("West")]
    //    West,
    //    [Description("South")]
    //    South,
    //}

    public enum MessageCategory
    {
        [Description("Warrnings")]
        Warrnings,
        [Description("Notifications")]
        Notifications,
        [Description("Alerts")]
        Alerts
    }

    public enum MessageDirection
    {
        [Description("Operation")]
        Operation,
        [Description("Observation")]
        Observation
    }

    public enum CommunicationType
    {
        [Description("Serial")]
        Serial = 0,
        [Description("Bluetooth")]
        Bluetooth = 1,
    }

    #region From Old Code
    public enum Command : byte { Packet = 1, Acknowledgement, Retry, CrcError, EndConversation }
    public enum BodyFlag : ushort
    {
        //None = 0,
        //TextObservation = 1,
        //LoginRequest = 2,
        //LoginResponse = 3,
        //AudibleObservation = 4,
        //VisualObservation = 5,
        //Lock = 6,
        //Destroy = 7,
        //ClientMsg = 8,
        //Deauthenticate = 9,
        //PlatoonMsg = 10,
        //ApplyFailoverMsg = 11,
        //FailoverResponseMsg = 12,
        //GsmConnNotification = 220,
        //MsgAcknowledged = 221,
        //GsmSmsResult = 222,
        //PhoneCall = 223,
        //KeepAlive = 224,
        //SynchTimeRequest = 225,
        //SynchTimeResponse = 226,
        //Location = 101,
        //GpsInProgress = 227,
        //MessagesStatusRequest = 228,
        //MessagesStatusResponse = 229,
        //Status = 230,
        //PhoneCallMissed = 231,
        //CallDestination = 232,
        //MsgReceived = 233,
        //AvailableCallingMethod = 234
        [Description("None")]
        None = 0,
        [Description("Text Observation")]
        TextObservation = 1,
        [Description("Login Request")]
        LoginRequest = 2,
        [Description("Login Response")]
        LoginResponse = 3,
        [Description("Audible Observation")]
        AudibleObservation = 4,
        [Description("Visual Observation")]
        VisualObservation = 5,
        [Description("Lock")]
        Lock = 6,
        [Description("Destroy")]
        Destroy = 7,
        [Description("Client Msg")]
        ClientMsg = 8,
        [Description("Deauthenticate")]
        Deauthenticate = 9,
        [Description("Platoon Msg")]
        PlatoonMsg = 10,
        [Description("Apply Failover Msg")]
        ApplyFailoverMsg = 11,
        [Description("Failover Response Msg")]
        FailoverResponseMsg = 12,
        [Description("Gsm Conn Notification")]
        GsmConnNotification = 220,
        [Description("Msg Acknowledged")]
        MsgAcknowledged = 221,
        [Description("Gsm Sms Result")]
        GsmSmsResult = 222,
        [Description("Phone Call")]
        PhoneCall = 223,
        [Description("Keep Alive")]
        KeepAlive = 224,
        [Description("Synch Time Request")]
        SynchTimeRequest = 225,
        [Description("Synch Time Response")]
        SynchTimeResponse = 226,
        [Description("Location")]
        Location = 101,
        [Description("Gps In Progress")]
        GpsInProgress = 227,
        [Description("Messages Status Request")]
        MessagesStatusRequest = 228,
        [Description("Messages Status Response")]
        MessagesStatusResponse = 229,
        [Description("Status")]
        Status = 230,
        [Description("Phone Call Missed")]
        PhoneCallMissed = 231,
        [Description("Call Destination")]
        CallDestination = 232,
        [Description("Msg Received")]
        MsgReceived = 233,
        [Description("Available Calling Method")]
        AvailableCallingMethod = 234
    }
    public enum LogType
    {
        StartOperation,
        StopOperation,
        Critical,
        Error,
        Information,
        Resume,
        Suspend,
        Transfer,
        Verbose,
        Warning,
    }
    public enum MessageType : ushort
    {
        [Description("None")]
        None = 0,
        [Description("Text Observation")]
        TextObservation = 1,
        [Description("Login Request")]
        LoginRequest = 2,
        [Description("Login Response")]
        LoginResponse = 3,
        [Description("Audible Observation")]
        AudibleObservation = 4,
        [Description("Visual Observation")]
        VisualObservation = 5,
        [Description("Lock")]
        Lock = 6,
        [Description("Destroy")]
        Destroy = 7,
        [Description("Client Msg")]
        ClientMsg = 8,
        [Description("Deauthenticate")]
        Deauthenticate = 9,
        [Description("Platoon Msg")]
        PlatoonMsg = 10,
        [Description("Apply Failover Msg")]
        ApplyFailoverMsg = 11,
        [Description("Failover Response Msg")]
        FailoverResponseMsg = 12,
        [Description("Gsm Conn Notification")]
        GsmConnNotification = 220,
        [Description("Msg Acknowledged")]
        MsgAcknowledged = 221,
        [Description("Gsm Sms Result")]
        GsmSmsResult = 222,
        [Description("Phone Call")]
        PhoneCall = 223,
        [Description("Keep Alive")]
        KeepAlive = 224,
        [Description("Synch Time Request")]
        SynchTimeRequest = 225,
        [Description("Synch Time Response")]
        SynchTimeResponse = 226,
        [Description("Location")]
        Location = 101,
        [Description("Gps In Progress")]
        GpsInProgress = 227,
        [Description("Messages Status Request")]
        MessagesStatusRequest = 228,
        [Description("Messages Status Response")]
        MessagesStatusResponse = 229,
        [Description("Status")]
        Status = 230,
        [Description("Phone Call Missed")]
        PhoneCallMissed = 231,
        [Description("Call Destination")]
        CallDestination = 232,
        [Description("Msg Received")]
        MsgReceived = 233,
        [Description("Available Calling Method")]
        AvailableCallingMethod = 234
    }
    public enum GsmMsgSendResult : byte
    {
        /// <summary>
        /// SMS Message faild to sent
        /// </summary>
        Fail,
        /// <summary>
        /// SMS Message sent
        /// </summary>
        Sent,
        /// <summary>
        /// SMS Message Acknowledged
        /// </summary>
        Acknowledged,
        /// <summary>
        /// Unknowen Message status
        /// </summary>
        None,
        /// <summary>
        /// SMS Message ready to send
        /// </summary>
        NotSent,
        /// <summary>
        /// SMS Message NotAcknowledged
        /// </summary>
        NotAcknowledged,
        /// <summary>
        /// SMS Message Canceled becuase It exceeds the number or trials to be sent
        /// </summary>
        Canceled,
        /// <summary>
        /// SMS Message Received - Client user Ack message
        /// </summary>
        Received,
    }
    public enum MsgType : byte
    {
        MissedCall,
        MissedSMS,
        HHUMsg,
        Guidance,
        PlatoonMsg
    }
    public enum StatusRequestType
    {
        LoginRequest,
        ConnectionStateChange,
        SingleStatusRequest,
        DecodePacket,
        DebugLoginRequest,
        StatusMessagesRequest
    }
    public enum RequestPacketTypes : uint
    {
        /// <summary>
        /// initial value, representing an empty request. or reinitializing a request
        /// </summary>
        Initial = 0x00000000,

        /// <summary>
        /// GSM Request bit position
        /// </summary>
        GSM = 0x00000001,

        /// <summary>
        /// Ring Request bit position
        /// </summary>
        Ring = 0x00000002,

        /// <summary>
        /// MissedCalls Request bit position
        /// </summary>
        MissedCalls = 0x00000004,

        /// <summary>
        /// Messages Request bit position
        /// </summary>
        Messages = 0x00000008,

        /// <summary>
        /// PlatoonMsgs Request bit position
        /// </summary>
        PlatoonMsgs = 0x00000010,

        /// <summary>
        /// SyncTime Request bit position
        /// </summary>
        SyncTime = 0x00000020,

        /// <summary>
        /// SyncTime Request bit position
        /// </summary>
        AvailableCallingMethod = 0x00000040,
    }
    public enum ObserverStatus : byte
    { Alarm = 1, Notification }

    //TODO: need to unified with GsmMsgSendResult beause it's the same enum
    public enum MessageStatus
    {
        /// <summary>
        /// SMS Message faild to sent
        /// </summary>
        Fail,
        /// <summary>
        /// SMS Message sent
        /// </summary>
        Sent,
        /// <summary>
        /// SMS Message Acknowledged
        /// </summary>
        Acknowledged,
        /// <summary>
        /// Unknowen Message status
        /// </summary>
        None,
        /// <summary>
        /// SMS Message ready to send
        /// </summary>
        NotSent,
        /// <summary>
        /// SMS Message NotAcknowledged
        /// </summary>
        NotAcknowledged,
        /// <summary>
        /// SMS Message Canceled
        /// </summary>
        Canceled,
        /// <summary>
        /// SMS Message Received
        /// </summary>
        Received,
    }
    public enum NumberofTargets : byte { Other = 0, One, Two, Many }
    public enum Altitude : byte { Other = 0, Low, Mid, High }
    public enum Heading : byte { Other = 0, WE, EW, SN, NS }
    public enum TargetType : byte { Other=0, MilitaryTrans, CivilTrans, FighterJet, Helicopter, UAV }
    public enum AvailableCallingMethod : byte
    {
        None,
        Gsm,
        Voip,
        Both
    }
    public enum DestinationCall : byte
    {
        CaptainGSM,
        DispatcherGSM,
        CaptainVoip,
        DispatcherVoip
    }
    #endregion

    #region Battery
    public enum BatteryStatus
    {
        [Description("Charging")]
        Charging,
        [Description("Discharging")]
        Discharging,
        [Description("Full")]
        Full,
        [Description("NotCharging")]
        NotCharging,
        [Description("Unknown")]
        Unknown
    }

    public enum PowerSource
    {
        [Description("Battery")]
        Battery,
        [Description("Ac")]
        Ac,
        [Description("Usb")]
        Usb,
        [Description("Wireless")]
        Wireless,
        [Description("Other")]
        Other
    }
    #endregion
}