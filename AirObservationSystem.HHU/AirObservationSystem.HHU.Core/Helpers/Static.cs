using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.Message;
using AirObservationSystem.HHU.Core.Model.Old_Code;
using AirObservationSystem.HHU.Core.Model.User;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.Core.ViewModels;
using AirObservationSystem.HHU.Core.Views;
using Autofac;
using Plugin.Toasts;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Helpers
{
    public static class Static
    {
        public static bool CommunicationTypeIsVisible { get; internal set; }
        public static ImageSource CommunicationType { get; internal set; }
        public static CommunicationType CurrentCommunicationType { get; internal set; }
        public static bool IsLocked { get; internal set; }
        public static bool IsRinging { get; internal set; }
        public static IMedia SerialPort { get; internal set; }
        public static IMedia Bluetooth { get; internal set; }
        public static bool KeepAlive { get; internal set; } = false;
        public static int KeepAliveInterval { get; internal set; } = 30; //TODO:Loaded from Settings 60 Seconds
        public static double ProgressBarMax { get; internal set; } = 60 * 50; //TODO:Loaded from Settings 50 minutes
        public static string StationName { get; internal set; } //TODO:Loaded from Settings
        public static ObservationType ObservationType { get; internal set; }
        public static MessageDirection MessageDirection { get; internal set; }
        public static string Message { get; internal set; }
        public static NumberofTargets ObservationNumber { get; internal set; }
        public static TargetType AirborneType { get; set; }
        public static Heading Heading { get; internal set; }
        public static Altitude Altitude { get; internal set; }
        public static MessageCategory MessageCategory { get; internal set; }
        public static string BluetoothName { get; set; } = "AOSBTKM4";//TODO:Loaded fom Settings
        public static string BluetoothPin { get; set; } = "1234";//TODO:Loaded fom Settings
        public static string BluetoothServiceUUId { get; set; } = "00001101-0000-1000-8000-00805f9b34fb";
        public static ushort BluetoothSdpServiceNameAttributeId { get; set; } = 0x100;
        public static byte BluetoothSdpServiceNameAttributeType { get; set; } = (4 << 3) | 5;
        
        public static User CurrentUser { get; internal set; } = new User() { };//TODO:Loaded fom Settings if any
        public static List<MessagesStatus> UnReadMessages { get; set; } = new List<MessagesStatus>();
        public static List<SentMessage> SentMessages { get; set; }
        public static string HHUId => "UWP";
        public static string DbName { get; set; } = "DB.db";
        public static AvailableCallingMethod AvailableCallingMethod { get; set; } = AvailableCallingMethod.None;
        public static DestinationCall DestinationCall { get; set; }

        public static void ChangeCurrentCultureInfo(CultureInfo cultureInfo)
        {
            System.Diagnostics.Debug.WriteLine("====== resource debug info =========");
            var assembly = typeof(App).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            System.Diagnostics.Debug.WriteLine("====================================");

            // This lookup NOT required for Windows platforms - the Culture will be automatically set
            //if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                // determine the correct, supported .NET culture
                //var ci = DependencyService.Get<ICultureInfo>().GetCurrentCultureInfo();
                Resx.Resources.Culture = cultureInfo; // set the RESX for resource localization
                AppContainer.Container.Resolve<ICultureInfo>().SetLocale(cultureInfo); // set the Thread for locale-aware methods
            }
        }

        public static void Exit()
        {
            AppContainer.Container.Resolve<IPlatform>().CloseApp();
        }

        public static ImageSource GetImageSource(string imageName)
        {
            ImageSource result = null;
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    result = ImageSource.FromFile(imageName);
                    break;
                case Device.Windows:
                    result = ImageSource.FromFile("Assets/Images/" + imageName);
                    break;
                case Device.iOS:
                    result = ImageSource.FromFile("Images/" + imageName);
                    break;
            }
            //return Device.OnPlatform(
            //                         iOS: ImageSource.FromFile("Images/" + imageName),
            //                         Android: ImageSource.FromFile(imageName),
            //                         WinPhone: ImageSource.FromFile("Assets/Images/" + imageName));
            return result;
        }

        public static async Task<INotificationResult> ShowToast(INotificationOptions options)
        {
            //var notificator = DependencyService.Get<IToastNotificator>();
            var notificator = AppContainer.Container.Resolve<IToastNotificator>();

            return await notificator.Notify(options);
        }

        public static void Log(string methodName, LogType logType, string description)
        {
            Debug.WriteLine($"{methodName};{logType.ToString()};{description}");
            //Logger.Write("Field IU", "AEC.AOS.IU_HHUProtocol", methodName, logType, DateTime.Now, description);
        }

        public static async Task PopPagesAsync(List<Type> pagesList)
        {
            var navigation =
                ((MainView<MainViewModel>)((NavigationPage)Application.Current.MainPage).CurrentPage).Navigation;

            var lst =
                navigation.NavigationStack.Where(
                    p =>
                        !pagesList.Contains(p.GetType())).ToList();
            for (var i = 0; i < lst.Count; i++)
                navigation.RemovePage(lst[i]);

            CurrentUser.IsLogedIn = false;

            await navigation.PopAsync();
        }

        public static void ChangeSystemTime(DateTime dt)
        {
            //TODO:Set New Time to the system

        }

        public static SQLiteAsyncConnection GetConnection(string path, ISQLitePlatform iSqLite)
        {
            var connectionFactory = new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(iSqLite, new SQLiteConnectionString(path, false)));
            return new SQLiteAsyncConnection(connectionFactory);
        }
    }


}
