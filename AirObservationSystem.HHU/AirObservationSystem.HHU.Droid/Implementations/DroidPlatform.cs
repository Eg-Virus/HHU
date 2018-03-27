using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.Droid.Implementations;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using System;
using System.IO;
using AirObservationSystem.HHU.Core.Helpers;
using Xamarin.Forms;
using Application = Android.App.Application;
using BatteryStatus = AirObservationSystem.HHU.Core.Helpers.BatteryStatus;
using Environment = System.Environment;

[assembly: Dependency(typeof(DroidPlatform))]

namespace AirObservationSystem.HHU.Droid.Implementations
{
    public class DroidPlatform : IPlatform
    {
        //WindowManagerFlags _originalFlags;

        public string GetModel()
        {
            return $"{Build.Manufacturer} {Build.Model}";
        }

        public string GetVersion()
        {
            return Build.VERSION.Release;
        }

        public void SetStatusBar(bool show)
        {
            //var activity = (Activity)Forms.Context;
            //var attrs = activity.Window.Attributes;
            //if (show)
            //{
            //    attrs.Flags = _originalFlags;
            //}
            //else
            //{
            //    _originalFlags = attrs.Flags;
            //    attrs.Flags |= Android.Views.WindowManagerFlags.Fullscreen;
            //}
            //activity.Window.Attributes = attrs;
            var activity = (Activity) Forms.Context;

            if (show)
                activity.Window.ClearFlags(WindowManagerFlags.Fullscreen);
            else
                activity.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }

        public void CloseApp()
        {
            throw new System.NotImplementedException();
        }

        public int RemainingChargePercent
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
                            var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);

                            return (int) Math.Floor(level * 100D / scale);
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }

            }
        }

        public BatteryStatus Status
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            //var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int) BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int) BatteryPlugged.Ac;
                            bool wirelessCharge = chargePlug == (int) BatteryPlugged.Wireless;

                            var isCharging = (usbCharge || acCharge || wirelessCharge);
                            if (isCharging)
                                return BatteryStatus.Charging;

                            switch (status)
                            {
                                case (int) BatteryStatus.Charging:
                                    return BatteryStatus.Charging;
                                case (int) BatteryStatus.Discharging:
                                    return BatteryStatus.Discharging;
                                case (int) BatteryStatus.Full:
                                    return BatteryStatus.Full;
                                case (int) BatteryStatus.NotCharging:
                                    return BatteryStatus.NotCharging;
                                default:
                                    return BatteryStatus.Unknown;
                            }
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public PowerSource PowerSource
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            //int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            //var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int) BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int) BatteryPlugged.Ac;

                            bool wirelessCharge = chargePlug == (int) BatteryPlugged.Wireless;

                            var isCharging = (usbCharge || acCharge || wirelessCharge);

                            if (!isCharging)
                                return PowerSource.Battery;
                            else if (usbCharge)
                                return PowerSource.Usb;
                            else if (acCharge)
                                return PowerSource.Ac;
                            else if (wirelessCharge)
                                return PowerSource.Wireless;
                            else
                                return PowerSource.Other;
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public string GetDbPath(string dbName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
        }
    }
}