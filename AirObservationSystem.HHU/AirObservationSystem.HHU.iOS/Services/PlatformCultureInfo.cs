using System;
using System.Threading;
using System.Globalization;
using Foundation;
using Xamarin.Forms;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.Core.Services;


[assembly: Dependency(typeof(AirObservationSystem.HHU.iOS.Services.PlatformCultureInfo))]
namespace AirObservationSystem.HHU.iOS.Services
{
    public class PlatformCultureInfo : ICultureInfo
    {
        //public System.Globalization.CultureInfo GetCurrentCultureInfo()
        //{
        //    var netLanguage = "en";
        //    var prefLanguageOnly = "en";
        //    if (NSLocale.PreferredLanguages.Length > 0)
        //    {
        //        var pref = NSLocale.PreferredLanguages[0];
        //        prefLanguageOnly = pref.Substring(0, 2);
        //        if (prefLanguageOnly == "pt")
        //        {
        //            if (pref == "pt")
        //                pref = "pt-BR"; // get the correct Brazilian language strings from the PCL RESX (note the local iOS folder is still "pt")
        //            else
        //                pref = "pt-PT"; // Portugal
        //        }
        //        netLanguage = pref.Replace("_", "-");
        //        Console.WriteLine("preferred language:" + netLanguage);
        //    }
        //    System.Globalization.CultureInfo ci = null;
        //    try
        //    {
        //        ci = new System.Globalization.CultureInfo(netLanguage);
        //    }
        //    catch
        //    {
        //        // iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
        //        // fallback to first characters, in this case "en"
        //        ci = new System.Globalization.CultureInfo(prefLanguageOnly);
        //    }
        //    return ci;
        //}

        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            Console.WriteLine(@"CurrentCulture set: " + ci.Name);
        }

        /// <remarks>
        /// TODO: Not sure if we can cache this info rather than querying every time
        /// </remarks>
        public CultureInfo GetCurrentCultureInfo()
        {
            var netLanguage = "en";
            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];

                netLanguage = iOSToDotnetLanguage(pref);
            }

            // this gets called a lot - try/catch can be expensive so consider caching or something
            CultureInfo ci ;
            try
            {
                ci = new CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException e1)
            {
                // iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
                // fallback to first characters, in this case "en"
                try
                {
                    var fallback = ToDotnetFallbackLanguage(new PCLCultureInfo(netLanguage));
                    Console.WriteLine(netLanguage + @" failed, trying " + fallback + @" (" + e1.Message + @")");
                    ci = new CultureInfo(fallback);
                }
                catch (CultureNotFoundException e2)
                {
                    // iOS language not valid .NET culture, falling back to English
                    Console.WriteLine(netLanguage + @" couldn't be set, using 'en' (" + e2.Message + @")");
                    ci = new CultureInfo("en");
                }
            }

            return ci;
        }

        string iOSToDotnetLanguage(string iosLanguage)
        {
            Console.WriteLine(@"iOS Language:" + iosLanguage);
            var netLanguage = iosLanguage;

            //certain languages need to be converted to CultureInfo equivalent
            switch (iosLanguage)
            {
                case "ms-MY":   // "Malaysian (Malaysia)" not supported .NET culture
                case "ms-SG":   // "Malaysian (Singapore)" not supported .NET culture
                    netLanguage = "ms"; // closest supported
                    break;
                case "gsw-CH":  // "Schwiizertüütsch (Swiss German)" not supported .NET culture
                    netLanguage = "de-CH"; // closest supported
                    break;
                    // add more application-specific cases here (if required)
                    // ONLY use cultures that have been tested and known to work
            }

            Console.WriteLine(@".NET Language/Locale:" + netLanguage);
            return netLanguage;
        }
        string ToDotnetFallbackLanguage(PCLCultureInfo platCulture)
        {
            Console.WriteLine(@".NET Fallback Language:" + platCulture.LanguageCode);
            var netLanguage = platCulture.LanguageCode; // use the first part of the identifier (two chars, usually);

            switch (platCulture.LanguageCode)
            {
                // 
                case "pt":
                    netLanguage = "pt-PT"; // fallback to Portuguese (Portugal)
                    break;
                case "gsw":
                    netLanguage = "de-CH"; // equivalent to German (Switzerland) for this app
                    break;
                    // add more application-specific cases here (if required)
                    // ONLY use cultures that have been tested and known to work
            }

            Console.WriteLine(@".NET Fallback Language/Locale:" + netLanguage + @" (application-specific)");
            return netLanguage;
        }
    }
}
