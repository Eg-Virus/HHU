
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.PlatformInterface
{
    public interface IMedia
    {
        //Events
        event EventHandler StartedListening;
        event EventHandler StoppedListening;
        event EventHandler<object> DataReceived;

        //Methods
        //Task<Result> LoadDeviceInformation();
        Task<Result> InitializeMediaAsync();
        Task<Result> SendAsync(object obj);
        Result Stop();

        //Properties
        //List<string> DeviceList { get; }

    }
}
