using System;
using Windows.Foundation.Metadata;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using Windows.UI.ViewManagement;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.UWP.Implementation;
using Xamarin.Forms;
using Application = Windows.UI.Xaml.Application;

[assembly: Dependency(typeof(UWPPlatform))]
namespace AirObservationSystem.HHU.UWP.Implementation
{
    public class UWPPlatform : IPlatform
    {
        private readonly EasClientDeviceInformation _devInfo = new EasClientDeviceInformation();
        //private BatteryStatus _status = BatteryStatus.Unknown;
        //Windows.Devices.Power.Battery _battery;

        public string GetModel()
        {
            return string.Format("{0} {1}", _devInfo.SystemManufacturer, _devInfo.SystemProductName);
        }

        public string GetVersion()
        {
            return _devInfo.OperatingSystem;
        }

        public void CloseApp()
        {
            Application.Current.Exit();
        }
        public async void SetStatusBar(bool show)
        {
            //PC customization
            //if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            //{
            //    var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            //    if (titleBar != null)
            //    {
            //        titleBar.ButtonBackgroundColor = Colors.DarkBlue;
            //        titleBar.ButtonForegroundColor = Colors.White;
            //        titleBar.BackgroundColor = Colors.Blue;
            //        titleBar.ForegroundColor = Colors.White;
            //    }
            //}

            //Mobile customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                    if (show)
                        await statusBar.ShowAsync();
                    else
                        await statusBar.HideAsync();
            }
        }

        public string GetDbPath(string dbName)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = folder.CreateFileAsync(dbName, CreationCollisionOption.OpenIfExists).GetAwaiter().GetResult();
            return file.Path;
            //return Path.Combine(ApplicationData.Current.LocalFolder.Path, dbName);
        }

        #region Battery
        //private Windows.Devices.Power.Battery DefaultBattery
        //{
        //    get
        //    {
        //        return _battery ?? (_battery = Windows.Devices.Power.Battery.AggregateBattery);
        //    }
        //}

        //public int RemainingChargePercent
        //{
        //    get
        //    {
        //        var finalReport = DefaultBattery.GetReport();
        //        var finalPercent = -1;

        //        if (finalReport.RemainingCapacityInMilliwattHours.HasValue && finalReport.FullChargeCapacityInMilliwattHours.HasValue)
        //        {
        //            finalPercent = (int)((finalReport.RemainingCapacityInMilliwattHours.Value /
        //                (double)finalReport.FullChargeCapacityInMilliwattHours.Value) * 100);
        //        }
        //        return finalPercent;
        //    }
        //}

        //public BatteryStatus Status
        //{
        //    get
        //    {
        //        var report = DefaultBattery.GetReport();
        //        var percentage = RemainingChargePercent;

        //        if (percentage >= 1.0)
        //        {
        //            _status = BatteryStatus.Full;
        //        }
        //        else if (percentage < 0)
        //        {
        //            _status = BatteryStatus.Unknown;
        //        }
        //        else
        //        {
        //            switch (report.Status)
        //            {
        //                case Windows.System.Power.BatteryStatus.Charging:
        //                    _status = BatteryStatus.Charging;
        //                    break;
        //                case Windows.System.Power.BatteryStatus.Discharging:
        //                    _status = BatteryStatus.Discharging;
        //                    break;
        //                case Windows.System.Power.BatteryStatus.Idle:
        //                    _status = BatteryStatus.NotCharging;
        //                    break;
        //                case Windows.System.Power.BatteryStatus.NotPresent:
        //                    _status = BatteryStatus.Unknown;
        //                    break;
        //            }
        //        }
        //        return _status;
        //    }
        //}

        //public PowerSource PowerSource
        //{
        //    get
        //    {
        //        if (_status == BatteryStatus.Full || _status == BatteryStatus.Charging)
        //        {
        //            return PowerSource.Ac;
        //        }
        //        return PowerSource.Battery;
        //    }
        //}

        #endregion

    }
}
