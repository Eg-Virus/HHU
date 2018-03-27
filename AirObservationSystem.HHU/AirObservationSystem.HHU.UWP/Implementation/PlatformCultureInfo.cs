using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.UWP.Implementation;
using Xamarin.Forms;

//[assembly: Dependency(typeof(PlatformCultureInfo))] //used Autofac
namespace AirObservationSystem.HHU.UWP.Implementation
{
    public class PlatformCultureInfo : ICultureInfo
    {
        //public System.Globalization.CultureInfo GetCurrentCultureInfo(string lang)
        //{
        //    return new System.Globalization.CultureInfo(lang);
        //}

        public System.Globalization.CultureInfo GetCurrentCultureInfo()
        {
            return System.Globalization.CultureInfo.CurrentCulture;
        }

        public void SetLocale(System.Globalization.CultureInfo ci)
        {
            // Do nothing
            System.Globalization.CultureInfo.CurrentCulture = ci;
        }
    }
}
