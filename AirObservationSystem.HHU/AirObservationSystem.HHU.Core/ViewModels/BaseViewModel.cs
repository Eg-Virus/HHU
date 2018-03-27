using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Sms;
using AirObservationSystem.HHU.Core.Helpers;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.Message;
using AirObservationSystem.HHU.Core.Model.Old_Code;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.Core.Repository.Message;
using AirObservationSystem.HHU.Core.Views.Pages;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Forms;
using Command = AirObservationSystem.HHU.Core.Helpers.Command;
using MessageType = AirObservationSystem.HHU.Core.Helpers.MessageType;

namespace AirObservationSystem.HHU.Core.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IViewModel
    {
        private readonly IPlatform _platform;
        private ICultureInfo _cultureInfo;
        private bool _isBusy;
        private string _title;
        private int KeepAliveInterval { get; set; }

        public BaseViewModel(IPlatform platform, ICultureInfo cultureInfo, IMedia media, ICrc32 crc32)
        {
            //_platform = platform;
            //_cultureInfo = cultureInfo;
            //_crc32 = crc32;



            _platform = AppContainer.Container.Resolve<IPlatform>();
            _cultureInfo = AppContainer.Container.Resolve<ICultureInfo>();

            //Media = media;
            //Media = DependencyService.Get<IMedia>();
            //Media.StartedListening -= OnStartedListening;
            //Media.StartedListening += OnStartedListening;
            //Media.StoppedListening += OnStoppedListening;
            //Media.StoppedListening -= OnStoppedListening;
            //Media.DataReceived -= OnDataReceived;
            //Media.DataReceived += OnDataReceived;


            Commands = new Dictionary<string, ICommand>();
        }

        public Dictionary<string, ICommand> Commands { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler StartedListening;
        public event EventHandler StoppedListening;
        public event EventHandler<object> DataReceivedLogin;
        public event EventHandler<object> DataReceivedConnectionChanged;
        public event EventHandler<object> DataReceivedDestinationCall;
        public event EventHandler<object> DataReceivedNotification;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RequestPacket StatusRequest(StatusRequestType statusRequestType, RequestPacketTypes singleType, uint raw)
        {
            RequestPacket requestPacket;
            switch (statusRequestType)
            {
                case StatusRequestType.LoginRequest:
                    requestPacket = new RequestPacket()
                        .StatusRequestSyncTime(DateTime.Now)
                        .StatusRequest(RequestPacketTypes.AvailableCallingMethod)
                        .StatusRequest(RequestPacketTypes.Messages)
                        .StatusRequest(RequestPacketTypes.PlatoonMsgs)
                        .StatusRequest(RequestPacketTypes.MissedCalls);
                    break;
                case StatusRequestType.ConnectionStateChange:
                    requestPacket = new RequestPacket()
                        .StatusRequest(RequestPacketTypes.GSM)
                        .StatusRequest(RequestPacketTypes.Messages)
                        .StatusRequest(RequestPacketTypes.PlatoonMsgs)
                        .StatusRequest(RequestPacketTypes.MissedCalls)
                        .StatusRequest(RequestPacketTypes.AvailableCallingMethod);
                    break;
                case StatusRequestType.SingleStatusRequest:
                    requestPacket = new RequestPacket(singleType);
                    break;
                case StatusRequestType.DecodePacket:
                    requestPacket = new RequestPacket(raw);
                    break;
                case StatusRequestType.DebugLoginRequest:
                    requestPacket = new RequestPacket()
                        .StatusRequestSyncTime(DateTime.Now)
                        .StatusRequest(RequestPacketTypes.AvailableCallingMethod)
                        .StatusRequest(RequestPacketTypes.Messages)
                        .StatusRequest(RequestPacketTypes.PlatoonMsgs);
                    break;
                case StatusRequestType.StatusMessagesRequest:
                    requestPacket = new RequestPacket()
                        .StatusRequest(RequestPacketTypes.MissedCalls)
                        .StatusRequest(RequestPacketTypes.Messages)
                        .StatusRequest(RequestPacketTypes.PlatoonMsgs);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statusRequestType), statusRequestType, null);
            }
            return requestPacket;
        }

        #region Local

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Platform Info

        public string GetModel => _platform.GetModel();
        public string GetVersion => _platform.GetVersion();

        public ICultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            set
            {
                _cultureInfo = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Media 

        public void SelectMedia()
        {
            Media = Static.CurrentCommunicationType == CommunicationType.Serial ? Static.SerialPort : Static.Bluetooth;

            Media.StartedListening -= OnStartedListening;
            Media.StartedListening += OnStartedListening;
            Media.StoppedListening += OnStoppedListening;
            Media.StoppedListening -= OnStoppedListening;
            Media.DataReceived -= OnDataReceived;
            Media.DataReceived += OnDataReceived;
        }

        public IMedia Media { get; set; }

        public Task<Result> InitializeMediaAsync() => Media.InitializeMediaAsync();

        public Task<Result> SendAsync(object obj, MessageType messageType)
        {
            CommandPacket requestMessage = null;

            switch (messageType)
            {
                case MessageType.None:
                    break;
                case MessageType.LoginResponse:
                    { break; }
                case MessageType.LoginRequest:
                    {
                        requestMessage = CommandPacketFactory.GetLoginRequest(hhuid: Static.HHUId,
                                                                              username: Static.CurrentUser.UserName,
                                                                              password: Static.CurrentUser.Password,
                                                                              retryCount: 1,
                                                                              loginRequestId: Static.CurrentUser.LogInMsgId);
                        requestMessage.Headers.AddHeader("MsgId", Static.CurrentUser.LogInMsgId.ToString());

                        break;
                    }
                case MessageType.TextObservation:
                    {
                        var parmList = (List<object>)obj;// it's the same order 
                        requestMessage =
                                CommandPacketFactory.GetTextObservation(observerStatus: (ObserverStatus)parmList[0],
                                                                        bodyText: parmList[1].ToString(),
                                                                        observerName: parmList[2].ToString(),
                                                                        retryCount: int.Parse(parmList[3].ToString()),
                                                                        dtObservationTime: (DateTime)parmList[4]);
                        //Add new message to SentMessages object
                        //Add new message to DB
                        //Add the ID from DB to the Header under MsgId
                        AddToSent(requestMessage, (DateTime)parmList[4]);

                        break;
                    }
                case MessageType.AudibleObservation:
                    {
                        var parmList = (List<object>)obj;// it's the same order 
                        requestMessage =
                                CommandPacketFactory.GetAudibleObservation(noOfTargets: (byte)parmList[0],
                                                                           targetType: (byte)parmList[1],
                                                                           altitude: (byte)parmList[2],
                                                                           heading: (byte)parmList[3],
                                                                           observerName: Static.CurrentUser.UserName,
                                                                           retryCount: 0,
                                                                           dtObservationTime: (DateTime)parmList[4]);
                        //Add new message to SentMessages object
                        //Add new message to DB
                        //Add the ID from DB to the Header under MsgId
                        AddToSent(requestMessage, (DateTime)parmList[4]);

                        break;
                    }
                case MessageType.VisualObservation:
                    {
                        var parmList = (List<object>)obj;// it's the same order 
                        requestMessage =
                                CommandPacketFactory.GetVisualObservation(noOfTargets: (byte)parmList[0],
                                                                           targetType: (byte)parmList[1],
                                                                           altitude: (byte)parmList[2],
                                                                           heading: (byte)parmList[3],
                                                                           observerName: Static.CurrentUser.UserName,
                                                                           retryCount: 0,
                                                                           dtObservationTime: (DateTime)parmList[4]);
                        //Add new message to SentMessages object
                        //Add new message to DB
                        //Add the ID from DB to the Header under MsgId
                        AddToSent(requestMessage, (DateTime)parmList[4]);

                        break;
                    }
                case MessageType.Lock:
                    {
                        requestMessage = CommandPacketFactory.GetPacket(new LockBody(Static.HHUId), 1);

                        break;
                    }
                case MessageType.Destroy:
                    { break; }
                case MessageType.ClientMsg:
                    { break; }
                case MessageType.Deauthenticate:
                    { break; }
                case MessageType.PlatoonMsg:
                    { break; }
                case MessageType.ApplyFailoverMsg:
                    { break; }
                case MessageType.FailoverResponseMsg:
                    { break; }
                case MessageType.GsmConnNotification:
                    { break; }
                case MessageType.MsgAcknowledged:
                    {
                        requestMessage = new CommandPacket(Command.Acknowledgement);
                        break;
                    }
                case MessageType.GsmSmsResult:
                    { break; }
                case MessageType.PhoneCall:
                    { break; }
                case MessageType.KeepAlive:
                    {
                        Debug.WriteLine("Send:KeepAlive");
                        requestMessage = CommandPacketFactory.GetKeepAlive();
                        break;
                    }
                case MessageType.SynchTimeRequest:
                    { break; }
                case MessageType.SynchTimeResponse:
                    { break; }
                case MessageType.Location:
                    { break; }
                case MessageType.GpsInProgress:
                    { break; }
                case MessageType.MessagesStatusRequest:
                    {
                        requestMessage = CommandPacketFactory.GetStatusRequest((RequestPacket)obj, 1);
                        break;
                    }
                case MessageType.MessagesStatusResponse:
                    { break; }
                case MessageType.Status:
                    { break; }
                case MessageType.PhoneCallMissed:
                    { break; }
                case MessageType.CallDestination:
                    {
                        requestMessage = CommandPacketFactory.GetCallDestination(Static.DestinationCall, 1);
                        break;
                    }
                case MessageType.MsgReceived:
                    { break; }
                case MessageType.AvailableCallingMethod:
                    { break; }
                default:
                    {
                        Static.Log("AirObservationSystem.HHU.Core.ViewModels.BaseViewModel.SendAsync", LogType.Critical, "Unknown Message Type");
                        break;
                    }
            }

            return requestMessage != null ? Media.SendAsync(requestMessage.AsByteArray()) : null;
        }

        private void AddToSent(CommandPacket cmdPacket, DateTime observationTime)
        {
            var msg = new SentMessage()
            {
                Status = MessageStatus.NotSent,
                Body =
                    cmdPacket.Body.TypeFlag.Description() != null
                        ? (cmdPacket.Body.TypeFlag.Description().Length > 0
                            ? cmdPacket.Body.TypeFlag.Description()
                            : cmdPacket.Body.TypeFlag.ToString())
                        : cmdPacket.Body.TypeFlag.ToString(),
                DateTime = observationTime
            };
            //if (messageType == MessageType.TextObservation)
            //    msg.DateTime = ((TextObservationBody)cmdPacket.Body).ObservationTime;
            //else if (messageType == MessageType.AudibleObservation)
            //    msg.DateTime = ((AudibleObservationBody)cmdPacket.Body).ObservationTime;
            //else if (messageType == MessageType.VisualObservation)
            //    msg.DateTime = ((VisualObservationBody)cmdPacket.Body).ObservationTime;

            AppContainer.Container.Resolve<SentMessageRepository>().InsertAsync(msg);
            Static.SentMessages.Add(msg);

            cmdPacket.Headers.AddHeader("MsgId", msg.Id.ToString());
        }

        private void OnStartedListening(object sender, EventArgs e)
        {
            Static.KeepAlive = true;
            KeepAliveInterval = Static.KeepAliveInterval;

            Device.StartTimer(TimeSpan.FromSeconds(1), SendKeepAlive);

            //remove if not needed
            StartedListening?.Invoke(this, e);
        }

        private bool SendKeepAlive()
        {

            if (KeepAliveInterval > 0 && Static.KeepAlive)
            {
                KeepAliveInterval -= 1;
                return true;
            }
            if (KeepAliveInterval == 0 && Static.KeepAlive)
            {
                KeepAliveInterval = Static.KeepAliveInterval;
                SendAsync(null, MessageType.KeepAlive);
                return true;
            }

            return false;
        }

        private void OnStoppedListening(object sender, EventArgs e)
        {
            Static.KeepAlive = false;

            StoppedListening?.Invoke(this, e);
        }

        private void OnDataReceived(object sender, object e)
        {
            var STR_RetryCount = "Count";
            #region Try
            try
            {
                var commandPacket = new CommandPacket((byte[])e);
                var cmd = commandPacket.Command;
                if (cmd == Command.Acknowledgement)
                {
                }
                #region CrcError
                else if (cmd == Command.CrcError)
                {
                    var retryCount = 0;
                    if (commandPacket.Headers.Count > 0 && commandPacket.Headers.ContainsName(STR_RetryCount))
                        retryCount = int.Parse(commandPacket.Headers.GetByName(STR_RetryCount).Value);
                    var retryPacket = new CommandPacket(Command.Retry);
                    retryPacket.Headers.AddHeader(STR_RetryCount, (++retryCount).ToString());
                    Media.SendAsync(retryPacket.AsByteArray()); //TODO:Double Check
                }
                #endregion
                else if (cmd == Command.Retry)
                {
                    //WriteMessage(endConvMessage);//TODO:Double Check
                }
                else if (cmd == Command.Packet)
                {
                    SendAsync(null, MessageType.MsgAcknowledged); // the constructin the send method
                    MessagesStatus msg;
                    switch (commandPacket.Body.TypeFlag)
                    {
                        #region Requests or None implementable because it's used from the other side.
                        case BodyFlag.ApplyFailoverMsg:
                            { break; }
                        case BodyFlag.None:
                            {
                                break;
                            }
                        case BodyFlag.LoginRequest:
                            {
                                break;
                            }
                        case BodyFlag.SynchTimeRequest:
                            { break; }
                        case BodyFlag.MessagesStatusRequest:
                            { break; }
                        case BodyFlag.TextObservation:
                            {
                                break;
                            }
                        case BodyFlag.AudibleObservation:
                            {
                                break;
                            }
                        case BodyFlag.VisualObservation:
                            {
                                break;
                            }
                        case BodyFlag.MsgAcknowledged:
                            { break; }
                        #endregion
                        #region ClientMsg
                        case BodyFlag.ClientMsg: //=Guidance, come from CLient in CIU
                            {
                                var body = (ClientMsgBody)commandPacket.Body;
                                //Show Notification for new message 
                                DataReceivedNotification?.Invoke(sender, MsgType.Guidance);
                                //TODO:Play sound 

                                //Check for duplicate message in the unread messages
                                msg = new MessagesStatus(MsgType.Guidance, body.Text, body.SendDate) { IsGuidanceMsg = true, SmsContent = body.Text };

                                if (Static.UnReadMessages.FindIndex(m => m.SmsContent == msg.SmsContent) == -1)
                                {
                                    Static.UnReadMessages.Add(msg);

                                    //add it to the DB
                                    AppContainer.Container.Resolve<RecievedMessageRepository>()
                                        .InsertAsync(new RecievedMessage()
                                        {
                                            Body = body.Text,
                                            DateTime = body.SendDate,
                                            MsgType = BodyFlag.ClientMsg,
                                        });
                                }
                                break;
                            }
                        #endregion
                        #region PlatoonMsg
                        case BodyFlag.PlatoonMsg:
                            {
                                var body = (PlatoonMsgBody)commandPacket.Body;
                                //Show Notification for new message 
                                DataReceivedNotification?.Invoke(sender, MsgType.Guidance);
                                //Play sound 

                                //Check for duplicate message in the unread messages
                                msg = new MessagesStatus(body.Text, body.PlatoonName, body.SendDate) { IsGuidanceMsg = true };
                                //TODO:Check if that true or not

                                if (Static.UnReadMessages.FindIndex(m => m.SmsContent == msg.SmsContent) == -1)
                                {
                                    Static.UnReadMessages.Add(msg);

                                    //add it to the DB
                                    AppContainer.Container.Resolve<RecievedMessageRepository>()
                                        .InsertAsync(new RecievedMessage()
                                        {
                                            Body = body.Text,
                                            DateTime = body.SendDate,
                                            MsgType = BodyFlag.ClientMsg,
                                        });
                                }
                                break;
                            }
                        #endregion
                        case BodyFlag.LoginResponse:
                            {
                                DataReceivedLogin?.Invoke(sender, commandPacket.Body);
                                break;
                            }
                        #region FIU in Control
                        case BodyFlag.Lock:
                            {
                                break;
                            }
                        case BodyFlag.Destroy:
                            {
                                //Destory the App mean close it;
                                Static.Exit();
                                break;
                            }
                        case BodyFlag.Deauthenticate:
                            {
                                Static.PopPagesAsync(new List<Type> { typeof(Login), typeof(CommunicationSelection) })
                                    .GetAwaiter();
                                break;
                            }
                        #endregion
                        case BodyFlag.FailoverResponseMsg:
                            { break; }
                        #region GsmConnNotification
                        case BodyFlag.GsmConnNotification:
                            {
                                DataReceivedConnectionChanged?.Invoke(sender, commandPacket.Body);
                                break;
                            }
                        #endregion
                        #region GsmSmsResult
                        case BodyFlag.GsmSmsResult:
                            {
                                long MsgId;
                                //TODO:Update the status of the sent MSGs from HHU and if the current page is reports update it
                                switch (((GsmMsgSendResultBody)commandPacket.Body).GsmSendResult)
                                {
                                    case GsmMsgSendResult.Fail:
                                        break;
                                    case GsmMsgSendResult.Sent: //Waiting for CIU to respones
                                        {
                                            MsgId = ((GsmMsgSendResultBody)commandPacket.Body).MsgID;
                                            if (Static.CurrentUser.IsLogedIn) //TODO: need handle in case is not LogedIn and recieved old confirmation from CIU
                                            {
                                                var sentMessage = Static.SentMessages.Find(m => m.Id == MsgId);
                                                if (sentMessage != null)
                                                {
                                                    sentMessage.Status = MessageStatus.Sent;
                                                    AppContainer.Container.Resolve<SentMessageRepository>()
                                                        .UpdateAsync(sentMessage);
                                                }
                                                else
                                                {
                                                    Static.Log("OnDataReceived", LogType.Critical, $"We have message with MsgID:{MsgId} that is not saved in DB");
                                                }
                                            }

                                            break;
                                        }
                                    case GsmMsgSendResult.Acknowledged:
                                        break;
                                    case GsmMsgSendResult.None:
                                        break;
                                    case GsmMsgSendResult.NotSent:
                                        break;
                                    case GsmMsgSendResult.NotAcknowledged:
                                        break;
                                    case GsmMsgSendResult.Canceled:
                                        break;
                                    case GsmMsgSendResult.Received: //respones from CIU 
                                        {
                                            MsgId = ((GsmMsgSendResultBody)commandPacket.Body).MsgID;
                                            if (Static.CurrentUser.IsLogedIn) //TODO: need handle in case is not LogedIn and recieved old confirmation from CIU
                                            {
                                                var sentMessage = Static.SentMessages.Find(m => m.Id == MsgId);
                                                if (sentMessage != null)
                                                {
                                                    sentMessage.Status = MessageStatus.Received;
                                                    AppContainer.Container.Resolve<SentMessageRepository>()
                                                        .UpdateAsync(sentMessage);
                                                }
                                                else
                                                {
                                                    Static.Log("OnDataReceived", LogType.Critical, $"We have message with MsgID:{MsgId} that is not saved in DB");
                                                }
                                            }
                                            break;
                                        }
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }


                                break;
                            }
                        #endregion
                        #region Phone Call
                        case BodyFlag.PhoneCall:
                            {
                                var body = (PhoneCallBody)commandPacket.Body;
                                switch (body.Status)
                                {
                                    case CallStatus.Ring:
                                        if (Static.CurrentUser.IsLogedIn && !Static.IsLocked)
                                        {
                                            var callerNumberData = body.CallerNumber.Split(',');
                                            //TODO:check in arabic and english for the array
                                            var callNumber = callerNumberData[0];
                                            if (!Static.IsRinging)
                                            {
                                                //TODO:Show a new page to interacte to the incoming call.
                                            }
                                        }
                                        break;
                                    case CallStatus.Started:
                                        //TODO:Close the calling page
                                        break;
                                    case CallStatus.Ended:
                                        break;
                                    case CallStatus.Missed:
                                        //TODO:Show notification for missed called and close the calling page if it still opened
                                        DataReceivedNotification?.Invoke(sender, body.Status);

                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                break;
                            }
                        #endregion
                        case BodyFlag.KeepAlive:
                            { break; }
                        #region Sync Time
                        case BodyFlag.SynchTimeResponse:
                            {
                                var body = (SynchTimeResponse)commandPacket.Body;
                                var t3 = DateTime.Now;
                                var ts1 = body.T1 - body.T0;
                                var ts2 = body.T2 - t3;
                                var diff = TimeSpan.FromMilliseconds((ts1 + ts2).TotalMilliseconds / 2.0);
                                var dt = (DateTime.Now + diff).ToUniversalTime();

                                Static.ChangeSystemTime(dt);
                                break;
                            }
                        #endregion
                        case BodyFlag.Location:
                            { break; }
                        #region GpsInProgress
                        case BodyFlag.GpsInProgress:
                            {
                                var body = (GpsInProgressRequest)commandPacket.Body;
                                if (body.GpsState)
                                {
                                    //TODO:show Page that show that the GPSInprogress
                                }
                                else
                                {
                                    //TODO:Close the page GPSInprogress
                                }
                                break;
                            }
                        #endregion
                        #region MessagesStatusResponse
                        case BodyFlag.MessagesStatusResponse:
                            {
                                var body = (MessagesStatusResponseBody)commandPacket.Body;

                                switch (body.MsgStausType)
                                {
                                    case MsgType.MissedCall:
                                        {
                                            //TODO: Check case PhoneCall.Missed
                                            break;
                                        }
                                    case MsgType.MissedSMS:
                                        {
                                            //TODO:show notifications for missed SMS

                                            //TODO:check duplicated sms and add to the unread

                                            break;
                                        }
                                    case MsgType.PlatoonMsg:
                                        {
                                            //TODO:show notifications for missed SMS

                                            //TODO:check duplicated sms and add to the unread
                                            break;
                                        }
                                    case MsgType.HHUMsg:
                                    case MsgType.Guidance: // come from CLient in CIU
                                        {

                                            break;
                                        }

                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                break;
                            }
                        #endregion
                        case BodyFlag.Status:
                            { break; }
                        #region PhoneCallMissed
                        case BodyFlag.PhoneCallMissed:
                            {
                                var body = (PhoneCallBody)commandPacket.Body;
                                switch (body.Status)
                                {
                                    case CallStatus.Ring:
                                        break;
                                    case CallStatus.Started:
                                        break;
                                    case CallStatus.Ended:
                                        break;
                                    case CallStatus.Missed:
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                break;
                            }
                        #endregion
                        #region CallDestination
                        case BodyFlag.CallDestination:
                            {
                                //TODO: change the drop down list of the calling destination
                                var body = (CallDestinationBody)commandPacket.Body;
                                switch (body.DestCall)
                                {
                                    case DestinationCall.CaptainGSM:
                                        break;
                                    case DestinationCall.DispatcherGSM:
                                        break;
                                    case DestinationCall.CaptainVoip:
                                        break;
                                    case DestinationCall.DispatcherVoip:
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                break;
                            }
                        #endregion
                        case BodyFlag.MsgReceived:
                            { break; }
                        #region AvailableCallingMethod
                        case BodyFlag.AvailableCallingMethod:
                            {
                                //TODO: check what's for?
                                Static.AvailableCallingMethod = ((AvailableCallingMethodBody)commandPacket.Body).AvaCall;
                                if (Static.AvailableCallingMethod == AvailableCallingMethod.Voip ||
                                    Static.AvailableCallingMethod == AvailableCallingMethod.Both)
                                {
                                    //Select DestinationCall.DispatcherVoip
                                    Static.DestinationCall = DestinationCall.DispatcherVoip;
                                    DataReceivedDestinationCall?.Invoke(sender, DestinationCall.DispatcherVoip);
                                }
                                else
                                {
                                    //Select DestinationCall.DispatcherGSM
                                    Static.DestinationCall = DestinationCall.DispatcherGSM;
                                    DataReceivedDestinationCall?.Invoke(sender, DestinationCall.DispatcherGSM);
                                }
                                break;
                            }
                        #endregion
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else if (cmd != Command.EndConversation)
                {
                    Static.Log("OnDataReceived", LogType.Warning,
                     $"Invalid Message Recieved: Command.EndConversation");
                }
            }
            #endregion
            #region Exceptions
            catch (EndOfStreamException eofse)
            {
                Static.Log("OnDataReceived", LogType.Warning,
                    $"Invalid Message Recieved: {eofse.Message}");
            }
            catch (InvalidDataException idexp)
            {
                Static.Log("OnDataReceived", LogType.Warning,
                   $"Invalid Message Recieved: {idexp.Message}");
            }
            #endregion
        }

        #endregion
    }
}