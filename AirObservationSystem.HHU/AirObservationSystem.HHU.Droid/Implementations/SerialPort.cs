using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirObservationSystem.HHU.Core.Helpers;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.Droid.Implementations;
using Xamarin.Forms;
using System.IO.Ports;
//[assembly: Dependency(typeof(SerialPort))]
namespace AirObservationSystem.HHU.Droid.Implementations
{
    //public class SerialPor : IMedia, INotifyPropertyChanged
    //{
    //    private CancellationTokenSource _readCancellationTokenSource;
    //    private Ports.SerialPort _serialPort;

    //    //private readonly ObservableCollection<DeviceInformation> _deviceList = new ObservableCollection<DeviceInformation>();
    //    //private string _deviceInformation;
    //    private DataWriter _dataWriteObject;
    //    private DataReader _dataReaderObject;

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    public event EventHandler StartedListening;
    //    public event EventHandler StoppedListening;
    //    public event EventHandler<object> DataReceived;

    //    protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    //    //public ObservableCollection<DeviceInformation> DeviceInformationList => _deviceList;

    //    //public byte[] ReceivedData
    //    //{
    //    //    get { return _receivedData; }
    //    //    set
    //    //    {
    //    //        _receivedData = value;
    //    //        OnPropertyChanged("ReceivedData");
    //    //    }
    //    //}

    //    public List<string> DeviceList { get; } = new List<string>();

    //    public async Task<Result> InitializeMediaAsync()
    //    {
    //        var result = Result.Fail;

    //        // Create cancellation token object to close I/O operations when closing the device
    //        _readCancellationTokenSource = new CancellationTokenSource();
    //        try
    //        {
    //            if (_serialPort == null)
    //            {
    //                await LoadDeviceInformation();
    //                _serialPort = await SerialDevice.FromIdAsync(DeviceList[0]);

    //                // Configure serial settings
    //                _serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
    //                _serialPort.BaudRate = 9600;
    //                _serialPort.Parity = SerialParity.None;
    //                _serialPort.StopBits = SerialStopBitCount.One;
    //                _serialPort.DataBits = 8;
    //                _serialPort.Handshake = SerialHandshake.None;
    //                _serialPort.ReadTimeout = TimeSpan.FromMilliseconds(200);

    //                // Create the DataWriter object and attach to OutputStream
    //                _dataWriteObject = new DataWriter(_serialPort.OutputStream)
    //                {
    //                    ByteOrder = ByteOrder.LittleEndian,
    //                    UnicodeEncoding = UnicodeEncoding.Utf8
    //                };

    //                Listen();
    //                result = Result.Success;
    //            }
    //            else
    //                result = Result.Success;
    //            StartedListening?.Invoke(null, null);
    //        }
    //        catch (Exception ex)
    //        {
    //            CloseDevice();

    //            Debug.Write(ex.Message);
    //        }
    //        return result;
    //    }

    //    public Result Stop()
    //    {
    //        var result = Result.Fail;
    //        try
    //        {
    //            CloseDevice();

    //            result = Result.Success;
    //        }
    //        catch (Exception e)
    //        {
    //            Static.Log("Stop", LogType.Error, e.Message);
    //        }
    //        return result;
    //    }

    //    public async Task<Result> SendAsync(object obj)
    //    {
    //        var result = Result.Fail;

    //        try
    //        {
    //            if (_serialPort != null)
    //            {
    //                if (_dataWriteObject == null)
    //                {
    //                    _dataWriteObject = new DataWriter(_serialPort.OutputStream)
    //                    {
    //                        ByteOrder = ByteOrder.LittleEndian,
    //                        UnicodeEncoding = UnicodeEncoding.Utf8
    //                    };
    //                }
    //                //Launch the WriteAsync task to perform the write
    //                await WriteAsync((byte[])obj);
    //                result = Result.Success;
    //            }
    //            else
    //            {
    //                Debug.Write("Select a device and connect");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.Write(ex.Message);
    //            CloseDevice();
    //        }

    //        return result;
    //    }

    //    #region Helpers
    //    private async Task<Result> LoadDeviceInformation()
    //    {
    //        var result = Result.Fail;

    //        try
    //        {
    //            //Load the available Serial Ports
    //            var aqs = SerialDevice.GetDeviceSelector();
    //            var dis = await DeviceInformation.FindAllAsync(aqs);
    //            foreach (DeviceInformation t in dis)
    //                DeviceList.Add(t.Id);

    //            //_deviceInformation = DeviceInformationList[0];

    //            result = Result.Success;
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.Write(ex.Message);
    //        }

    //        return result;
    //    }

    //    private async void Listen()
    //    {

    //        try
    //        {
    //            if (_serialPort != null)
    //            {
    //                if (_dataReaderObject == null)
    //                {
    //                    _dataReaderObject = new DataReader(_serialPort.InputStream)
    //                    {
    //                        UnicodeEncoding = UnicodeEncoding.Utf8,
    //                        ByteOrder = ByteOrder.LittleEndian
    //                    };
    //                }
    //                // keep reading the serial input
    //                while (true)
    //                {
    //                    await ReadAsync(_readCancellationTokenSource.Token);
    //                }

    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.Write(ex.GetType().Name == "TaskCanceledException" ?
    //                                              "Reading task was cancelled, closing device and cleaning up" : ex.Message);
    //            CloseDevice();
    //        }
    //    }

    //    private async Task WriteAsync(byte[] obj)
    //    {
    //        if (obj != null)
    //        {
    //            _dataWriteObject.WriteBytes(obj);

    //            // Launch an async task to complete the write operation
    //            var storeAsyncTask = _dataWriteObject.StoreAsync().AsTask();

    //            await storeAsyncTask;
    //        }
    //    }

    //    private async Task ReadAsync(CancellationToken cancellationToken)
    //    {
    //        uint readBufferLength = 2048;
    //        // If task cancellation was requested, comply
    //        cancellationToken.ThrowIfCancellationRequested();

    //        // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
    //        _dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;
    //        Debug.WriteLine("LoadAsync: Called");
    //        // Create a task object to wait for data on the serialPort.InputStream
    //        var noOfBytesToRead = await _dataReaderObject.LoadAsync(readBufferLength).AsTask(cancellationToken);
    //        Debug.WriteLine("LoadAsync: Answered");

    //        var bytes = new byte[noOfBytesToRead];

    //        //if (noOfBytesToRead > 0 && noOfBytesToRead == _dataReaderObject.UnconsumedBufferLength)
    //        if (noOfBytesToRead > 0)
    //        {
    //            _dataReaderObject.ReadBytes(bytes);

    //            int messageSize = BitConverter.ToInt16(bytes, 0); //Get the Message Size from the first 2 bytes

    //            if (messageSize == bytes.Length)        //One Message
    //                DataReceived?.Invoke(null, bytes);
    //            else                                    //More than one Message
    //            {
    //                var messasgesList = new List<byte[]>();

    //                var multiMessages = new byte[bytes.Length];
    //                Array.Copy(bytes, 0, multiMessages, 0, bytes.Length);

    //                while (multiMessages.Length >= messageSize)
    //                {
    //                    var messagePart = new byte[messageSize];
    //                    Array.Copy(multiMessages, 0, messagePart, 0, messageSize);// Get always the first message in the array until it will be one

    //                    messasgesList.Add(messagePart);
    //                    if (multiMessages.Length == messageSize) break;

    //                    var temp = new byte[multiMessages.Length - messageSize];
    //                    Array.Copy(multiMessages, messageSize, temp, 0, multiMessages.Length - messageSize);
    //                    multiMessages = temp.ToArray();
    //                    messageSize = BitConverter.ToInt16(multiMessages, 0); //Get the Message Size from the first 2 bytes
    //                }
    //                foreach (var message in messasgesList)
    //                {
    //                    DataReceived?.Invoke(null, message);
    //                }
    //            }
    //        }
    //        else
    //        { }
    //        //if (bytesRead > 0)
    //        //{
    //        //    bytes = new byte[bytesRead];
    //        //    _dataReaderObject.ReadBytes(bytes);
    //        //    DataReceived?.Invoke(null, bytes);
    //        //}
    //    }

    //    private void CancelReadTask()
    //    {
    //        if (_readCancellationTokenSource != null)
    //        {
    //            if (!_readCancellationTokenSource.IsCancellationRequested)
    //            {
    //                _readCancellationTokenSource.Cancel();
    //            }
    //        }
    //    }

    //    private void CloseDevice()
    //    {
    //        try
    //        {
    //            CancelReadTask();
    //            if (_dataWriteObject != null)
    //            {
    //                _dataWriteObject.DetachStream();
    //                _dataWriteObject.Dispose();
    //                _dataWriteObject = null;
    //            }
    //            if (_dataReaderObject != null)
    //            {
    //                _dataReaderObject.DetachStream();
    //                _dataReaderObject.Dispose();
    //                _dataReaderObject = null;
    //            }
    //            _serialPort?.Dispose();
    //            _serialPort = null;
    //            DeviceList.Clear();
    //            StoppedListening?.Invoke(null, null);
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.Write(ex.Message);
    //        }
    //    }



    //    #endregion
    //}
}