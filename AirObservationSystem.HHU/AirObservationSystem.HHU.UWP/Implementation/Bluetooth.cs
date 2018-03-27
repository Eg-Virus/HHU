using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using AirObservationSystem.HHU.Core.Helpers;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.UWP.Helpers;
using AirObservationSystem.HHU.UWP.Implementation;
using Xamarin.Forms;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Static = AirObservationSystem.HHU.Core.Helpers.Static;

//[assembly: Dependency(typeof(Bluetooth))]
namespace AirObservationSystem.HHU.UWP.Implementation
{
    public class Bluetooth : IMedia, INotifyPropertyChanged
    {
        //public ObservableCollection<DeviceInformationDisplay> ResultCollection { get; private set; } = new ObservableCollection<DeviceInformationDisplay>();
        private BluetoothDevice _bluetoothDevice = null;
        private RfcommDeviceService _service = null;
        private StreamSocket _socket = null;
        private DataWriter _writer = null;
        private DataReader _reader = null;
        private bool _cancel = false;

        public event EventHandler<object> DataReceived;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler StartedListening;
        public event EventHandler StoppedListening;

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<DeviceInformationDisplay> DeviceList { get; } = new ObservableCollection<DeviceInformationDisplay>();

        public async Task<Result> InitializeMediaAsync()
        {
            var result = Result.Fail;
            try
            {
                var deviceSelectorInfo = DeviceSelectorChoices.Bluetooth;
                // Paired or UnPaired.
                var selector = "(" + deviceSelectorInfo.Selector + ")" +
                               " AND (System.Devices.Aep.CanPair:=System.StructuredQueryType.Boolean#True " +
                               "OR System.Devices.Aep.IsPaired:=System.StructuredQueryType.Boolean#True)";
                var additionalProperties = new string[] { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

                // Kind is specified in the selector info
                var deviceWatcher = DeviceInformation.CreateWatcher(aqsFilter: selector,
                                                                    additionalProperties: additionalProperties,
                                                                    kind: deviceSelectorInfo.Kind);

                // Hook up handlers for the watcher events before starting the watcher
                deviceWatcher.Added += HandlerAdded;
                deviceWatcher.Updated += HandlerUpdated;
                deviceWatcher.Removed += HandlerRemoved;
                deviceWatcher.EnumerationCompleted += HandlerEnumCompleted;
                deviceWatcher.Stopped += HandlerStopped;

                deviceWatcher.Start();

                _cancel = false;
                await Task.Run(() =>
                {
                    WaitForDevice();
                });

                if (_socket != null)
                    result = Result.Success;

            }
            catch (Exception ex)
            {
                Static.Log("InitializeMediaAsync", LogType.Error,
                   $"Couldn't Initialize the Bluetooth Media: {ex.Message}");
            }

            return result;
        }

        public async Task<Result> SendAsync(object obj)
        {
            var result = Result.Fail;
            if (_bluetoothDevice.ConnectionStatus == BluetoothConnectionStatus.Connected)
            {
                try
                {
                    _writer.WriteBytes((byte[])obj);
                    await _writer.StoreAsync();

                    result = Result.Success;
                }
                catch (Exception ex)
                {
                    Static.Log("SendAsync", LogType.Error,
                        $"Couldn't Sent through Bluetooth Media: {ex.Message}");
                }
            }
            else
            {
                await InitializeMediaAsync();
                await SendAsync(obj);
            }
            return result;
        }

        #region Helpers

        private bool WaitForDevice()
        {
            while (true)
            {
                if (_cancel)
                    return false;
                if (_socket != null)
                    return true;
            }
        }

        private void HandlerStopped(DeviceWatcher watcher, object args)
        {
            //_cancel = true;
            //NotifyUser(
            //            String.Format("{0} devices found. Watcher {1}.",
            //                ResultCollection.Count,
            //                DeviceWatcherStatus.Aborted == watcher.Status ? "aborted" : "stopped"),
            //            NotifyType.StatusMessage);
        }

        private void HandlerEnumCompleted(DeviceWatcher sender, object args)
        {
            //_cancel = true;
            //NotifyUser(
            //            String.Format("{0} devices found. Enumeration completed. Watching for updates...", ResultCollection.Count),
            //            NotifyType.StatusMessage);
        }

        private void HandlerRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            // Find the corresponding DeviceInformation in the collection and remove it
            foreach (var deviceInfoDisp in DeviceList)
            {
                if (deviceInfoDisp.Id == deviceInfoUpdate.Id)
                {
                    DeviceList.Remove(deviceInfoDisp);
                    break;
                }
            }
        }

        private void HandlerUpdated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            // Find the corresponding updated DeviceInformation in the collection and pass the update object
            // to the Update method of the existing DeviceInformation. This automatically updates the object
            // for us.
            foreach (var deviceInfoDisp in DeviceList)
            {
                if (deviceInfoDisp.Id == deviceInfoUpdate.Id)
                {
                    deviceInfoDisp.Update(deviceInfoUpdate);
                    break;
                }
            }
        }

        private async void HandlerAdded(DeviceWatcher watcher, DeviceInformation deviceInfo)
        {
            if (DeviceList.All(deviceInfoDisp => deviceInfoDisp.Id != deviceInfo.Id))
                DeviceList.Add(new DeviceInformationDisplay(deviceInfo));

            var device = new DeviceInformationDisplay(deviceInfo);
            if (string.Equals(device.Name, Static.BluetoothName, StringComparison.CurrentCultureIgnoreCase))
            {
                DeviceAccessStatus accessStatus = DeviceAccessInformation.CreateFromId(device.Id).CurrentStatus;
                if (accessStatus != DeviceAccessStatus.DeniedBySystem || accessStatus != DeviceAccessStatus.DeniedByUser)
                {
                    Static.Log("HandlerAdded", LogType.Information, $"Device Name: {device.Name}");
                    Static.Log("HandlerAdded", LogType.Information, $"Device ID: {device.Id}");
                    Static.Log("HandlerAdded", LogType.Information, $"Device Access Status: {accessStatus}");

                    try
                    {
                        _bluetoothDevice = await BluetoothDevice.FromIdAsync(device.Id);
                    }
                    catch (Exception ex)
                    {
                        Static.Log("HandlerAdded", LogType.Error, $"Couldn't Get BluetoothDevice: {ex.Message}");
                        return;
                    }

                    Static.Log("HandlerAdded", LogType.Information, $"Device CanPair: {device.CanPair}");


                    if (device.CanPair)
                    {
                        //StopWatcher(watcher);

                        //Pair the device
                        // Get ceremony type and protection level selections
                        DevicePairingKinds ceremoniesSelected = DevicePairingKinds.ConfirmOnly;
                        DevicePairingProtectionLevel protectionLevel = DevicePairingProtectionLevel.Default;
                        DeviceInformationCustomPairing customPairing =
                            _bluetoothDevice.DeviceInformation.Pairing.Custom;

                        customPairing.PairingRequested += PairingRequestedHandler;
                        DevicePairingResult result = await customPairing.PairAsync(ceremoniesSelected,
                            protectionLevel);
                        customPairing.PairingRequested -= PairingRequestedHandler;

                        //DevicePairingResult result = await bluetoothDevice.DeviceInformation.Pairing.PairAsync();

                        if (result.Status == DevicePairingResultStatus.Paired)
                            DeviceList.Add(device);
                    }
                    if (device.IsPaired)
                    {
                        if (_bluetoothDevice != null)
                        {
                            StopWatcher(watcher, false);

                            var rfcommServices = await _bluetoothDevice.GetRfcommServicesForIdAsync(
                                RfcommServiceId.FromUuid(Guid.Parse(Static.BluetoothServiceUUId)), BluetoothCacheMode.Uncached);
                            if (rfcommServices.Services.Count > 0)
                            {
                                _service = rfcommServices.Services[0];
                                // Do various checks of the SDP record to make sure you are talking to a device that actually supports the Bluetooth Rfcomm Chat Service
                                var attributes = await _service.GetSdpRawAttributesAsync();
                                if (attributes.ContainsKey(Static.BluetoothSdpServiceNameAttributeId))
                                {
                                    var attributeReader =
                                        DataReader.FromBuffer(
                                            attributes[Static.BluetoothSdpServiceNameAttributeId]);
                                    var attributeType = attributeReader.ReadByte();
                                    if (attributeType == Static.BluetoothSdpServiceNameAttributeType)
                                    {
                                        var serviceNameLength = attributeReader.ReadByte();
                                        attributeReader.UnicodeEncoding = UnicodeEncoding.Utf8;



                                        lock (this)
                                        {
                                            _socket = new StreamSocket();
                                        }
                                        try
                                        {
                                            await _socket.ConnectAsync(_service.ConnectionHostName,
                                                _service.ConnectionServiceName);

                                            var serviceName = attributeReader.ReadString(serviceNameLength);

                                            _writer = new DataWriter(_socket.OutputStream);

                                            DataReader chatReader = new DataReader(_socket.InputStream);
                                            ReceiveStringLoop(chatReader);

                                            StopWatcher(watcher, true);

                                        }
                                        catch (Exception ex) when ((uint)ex.HResult == 0x80070490
                                        ) // ERROR_ELEMENT_NOT_FOUND
                                        {
                                            Static.Log("HandlerAdded", LogType.Error,
                                                $"Please verify that you are running the BluetoothRfcommChat: {ex.Message}");
                                        }
                                        catch (Exception ex) when ((uint)ex.HResult == 0x80072740) // WSAEADDRINUSE
                                        {
                                            Static.Log("HandlerAdded", LogType.Error,
                                                $"Please verify that there is no other RFCOMM connection to the same device: {ex.Message}");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #region Commented
                    /////////////////////////////////////////////////////////Need to be checked for its values
                    //    var serviceUuid = Guid.Parse(Static.BluetoothServiceUUId);
                    //    UInt16 SdpServiceNameAttributeId = 0x100;
                    //    byte SdpServiceNameAttributeType = (4 << 3) | 5;
                    //    ///////////////////////////////////////////////////////////
                    //    var rfcommServices = await bluetoothDevice.GetRfcommServicesAsync();

                    //    //if (rfcommServices.Services.Count == 0)
                    //    //{
                    //    //    rfcommServices = await
                    //    //    bluetoothDevice.GetRfcommServicesForIdAsync(RfcommServiceId.FromUuid(serviceUuid),
                    //    //                                                BluetoothCacheMode.Uncached);
                    //    //    //if (rfcommServices.Services.Count == 0)
                    //    //    //{
                    //    //    //    serviceUuid = Guid.Parse("FF00FCDC-289E-4996-A7B9-464670E3F691");

                    //    //    //    rfcommServices = await
                    //    //    //        bluetoothDevice.GetRfcommServicesForIdAsync(RfcommServiceId.SerialPort,
                    //    //    //                                                    BluetoothCacheMode.Uncached);
                    //    //    //    if (rfcommServices.Services.Count == 0)
                    //    //    //    {
                    //    //    //        rfcommServices = await
                    //    //    //            bluetoothDevice.GetRfcommServicesForIdAsync(RfcommServiceId.FromUuid(serviceUuid),
                    //    //    //                BluetoothCacheMode.Uncached);
                    //    //    //    }
                    //    //    //}
                    //    //}

                    //    if (rfcommServices.Services.Count > 0)
                    //    {
                    //        _service = rfcommServices.Services[0];

                    //        //var attributes = await _service.GetSdpRawAttributesAsync();
                    //        //if (attributes.ContainsKey(SdpServiceNameAttributeId))
                    //        //{
                    //        //    var attributeReader = DataReader.FromBuffer(attributes[SdpServiceNameAttributeId]);
                    //        //    var attributeType = attributeReader.ReadByte();
                    //        //    if (attributeType == SdpServiceNameAttributeType)
                    //        //    {
                    //        //        var serviceNameLength = attributeReader.ReadByte();

                    //                // The Service Name attribute requires UTF-8 encoding.
                    //                //attributeReader.UnicodeEncoding = UnicodeEncoding.Utf8;


                    //                lock (this)
                    //                {
                    //                    socket = new StreamSocket();
                    //                }
                    //                try
                    //                {
                    //                    await socket.ConnectAsync(_service.ConnectionHostName,
                    //                        _service.ConnectionServiceName);

                    //                    _writer = new DataWriter(socket.OutputStream);

                    //                    _reader = new DataReader(socket.InputStream);
                    //                    ReceiveStringLoop(_reader);
                    //                }
                    //                catch (Exception ex) when ((uint)ex.HResult == 0x80070490)
                    //                // ERROR_ELEMENT_NOT_FOUND
                    //                {
                    //                    //rootPage.NotifyUser("Please verify that you are running the BluetoothRfcommChat server.", NotifyType.ErrorMessage);
                    //                    Static.Log("HandlerAdded", LogType.Error, $"Please verify that you are running the BluetoothRfcommChat: {ex.Message}");
                    //                }
                    //                catch (Exception ex) when ((uint)ex.HResult == 0x80072740) // WSAEADDRINUSE
                    //                {
                    //                    //rootPage.NotifyUser("Please verify that there is no other RFCOMM connection to the same device.", NotifyType.ErrorMessage);
                    //                    Static.Log("HandlerAdded", LogType.Error, $"Please verify that there is no other RFCOMM connection to the same device: {ex.Message}");

                    //                }

                    //                /////////////////////////////////////
                    //                StopWatcher(watcher);

                    //        //    }
                    //        //}
                    //    } 
                    #endregion
                }
            }
        }

        private void StopWatcher(DeviceWatcher deviceWatcher, bool removeHandlers)
        {
            if (null != deviceWatcher)
            {
                // First unhook all event handlers except the stopped handler. This ensures our
                // event handlers don't get called after stop, as stop won't block for any "in flight" 
                // event handler calls.  We leave the stopped handler as it's guaranteed to only be called
                // once and we'll use it to know when the query is completely stopped. 
                if (removeHandlers)
                {
                    deviceWatcher.Added -= HandlerAdded;
                    deviceWatcher.Updated -= HandlerUpdated;
                    deviceWatcher.Removed -= HandlerRemoved;
                    deviceWatcher.EnumerationCompleted -= HandlerEnumCompleted;
                }
                if (DeviceWatcherStatus.Started == deviceWatcher.Status ||
                    DeviceWatcherStatus.EnumerationCompleted == deviceWatcher.Status)
                {
                    deviceWatcher.Stop();
                }
                deviceWatcher = null;
            }
        }

        private async void PairingRequestedHandler(DeviceInformationCustomPairing sender, DevicePairingRequestedEventArgs args)
        {
            switch (args.PairingKind)
            {
                case DevicePairingKinds.ConfirmOnly:
                    // Windows itself will pop the confirmation dialog as part of "consent" if this is running on Desktop or Mobile
                    // If this is an App for 'Windows IoT Core' where there is no Windows Consent UX, you may want to provide your own confirmation.
                    args.Accept();
                    break;

                case DevicePairingKinds.DisplayPin:
                    // We just show the PIN on this side. The ceremony is actually completed when the user enters the PIN
                    // on the target device. We automatically except here since we can't really "cancel" the operation
                    // from this side.
                    args.Accept();

                    // No need for a deferral since we don't need any decision from the user

                    break;

                case DevicePairingKinds.ProvidePin:
                    // A PIN may be shown on the target device and the user needs to enter the matching PIN on 
                    // this Windows device. Get a deferral so we can perform the async request to the user.
                    var collectPinDeferral = args.GetDeferral();

                    args.Accept(Static.BluetoothPin);

                    collectPinDeferral.Complete();

                    break;

                case DevicePairingKinds.ConfirmPinMatch:
                    // We show the PIN here and the user responds with whether the PIN matches what they see
                    // on the target device. Response comes back and we set it on the PinComparePairingRequestedData
                    // then complete the deferral.
                    var displayMessageDeferral = args.GetDeferral();

                    args.Accept(Static.BluetoothPin);

                    displayMessageDeferral.Complete();
                    break;
                case DevicePairingKinds.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async void ReceiveStringLoop(DataReader reader)
        {
            try
            {
                uint noOfBytesToRead = await reader.LoadAsync(sizeof(uint));
                if (noOfBytesToRead < sizeof(uint))
                {
                    Disconnect("Remote device terminated connection - make sure only one instance of server is running on remote device");
                    return;
                }
                var bytes = new byte[noOfBytesToRead];
                if (noOfBytesToRead > 0)
                {
                    reader.ReadBytes(bytes);

                    int messageSize = BitConverter.ToInt16(bytes, 0); //Get the Message Size from the first 2 bytes

                    if (messageSize == bytes.Length)        //One Message
                        DataReceived?.Invoke(null, bytes);
                    else                                    //More than one Message
                    {
                        var messasgesList = new List<byte[]>();

                        var multiMessages = new byte[bytes.Length];
                        Array.Copy(bytes, 0, multiMessages, 0, bytes.Length);

                        while (multiMessages.Length >= messageSize)
                        {
                            var messagePart = new byte[messageSize];
                            Array.Copy(multiMessages, 0, messagePart, 0, messageSize);// Get always the first message in the array until it will be one

                            messasgesList.Add(messagePart);
                            if (multiMessages.Length == messageSize) break;

                            var temp = new byte[multiMessages.Length - messageSize];
                            Array.Copy(multiMessages, messageSize, temp, 0, multiMessages.Length - messageSize);
                            multiMessages = temp.ToArray();
                            messageSize = BitConverter.ToInt16(multiMessages, 0); //Get the Message Size from the first 2 bytes
                        }
                        foreach (var message in messasgesList)
                        {
                            DataReceived?.Invoke(null, message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lock (this)
                {
                    if (_socket == null)
                    {
                        // Do not print anything here -  the user closed the socket.
                        if ((uint)ex.HResult == 0x80072745)
                            Static.Log("ReceiveStringLoop", LogType.Error, $"Disconnect triggered by remote device: {ex.Message}");
                        else if ((uint)ex.HResult == 0x800703E3)
                            Static.Log("ReceiveStringLoop", LogType.Error, $"The I/O operation has been aborted because of either a thread exit or an application request: {ex.Message}");

                    }
                    else
                    {
                        Disconnect("Read stream failed with error: " + ex.Message);
                    }
                }
            }
        }

        private void Disconnect(string disconnectReason)
        {
            if (_writer != null)
            {
                _writer.DetachStream();
                _writer = null;
            }

            if (_reader != null)
            {
                _reader.DetachStream();
                _reader = null;
            }

            if (_service != null)
            {
                _service.Dispose();
                _service = null;
            }
            lock (this)
            {
                if (_socket != null)
                {
                    _socket.Dispose();
                    _socket = null;
                }
            }
            Static.Log("Disconnect", LogType.Error, disconnectReason);

            //rootPage.NotifyUser(disconnectReason, NotifyType.StatusMessage);
            //ResetMainUI();
        }

        public Result Stop()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
