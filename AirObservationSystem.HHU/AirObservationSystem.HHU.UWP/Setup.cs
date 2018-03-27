using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.UWP.Implementation;
using Autofac;
using Plugin.Toasts;
using Plugin.Toasts.UWP;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;

namespace AirObservationSystem.HHU.UWP
{
    public class Setup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder cb)
        {
            base.RegisterDependencies(cb);

            cb.RegisterInstance(new SQLitePlatformWinRT()).As<ISQLitePlatform>();

            cb.RegisterType<PlatformCultureInfo>().As<ICultureInfo>();
            cb.RegisterType<UWPPlatform>().As<IPlatform>();

            cb.RegisterType<SerialPort>().As<IMedia>().Keyed<IMedia>("Serial");
            cb.RegisterType<Bluetooth>().As<IMedia>().Keyed<IMedia>("Bluetooth");

            cb.RegisterType<Crc32>().As<ICrc32>();

            //Toasts Notification Plugin
            //DependencyService.Register<ToastNotification>(); // Register your dependency
            cb.RegisterType<ToastNotification>().As<IToastNotificator>();
            ToastNotification.Init(); //you can pass additional parameters here
        }
    }
    
}