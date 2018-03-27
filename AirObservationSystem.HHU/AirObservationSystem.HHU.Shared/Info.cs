#if __IOS__ 
using UIKit;
#elif __ANDROID__ 
using Android.OS;
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP 
using Windows.Security.ExchangeActiveSyncProvisioning;
#endif

namespace AirObservationSystem.HHU.Shared
{
    public static class Info
    {
        public static string Model
        {
            get
            {
#if __IOS__
            return (new UIDevice()).Model.ToString(); 
#elif __ANDROID__
                return string.Format("{0} {1}", Build.Manufacturer, Build.Model);
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                EasClientDeviceInformation devInfo = new EasClientDeviceInformation();
                return string.Format("{0} {1}", devInfo.SystemManufacturer, devInfo.SystemProductName);
#else
                return "Unknown Model.";
#endif
            }
        }

        public static string Version
        {
            get
            {
#if __IOS__
                        UIDevice 
                        device = new UIDevice(); 
      return string.Format("{0} {1}", device.SystemName, device.SystemVersion);
#elif __ANDROID__
                return Build.VERSION.Release.ToString();
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                return new EasClientDeviceInformation().OperatingSystem;
#else
                return "Unknown Version.";
#endif

            }
        }
    }
}
