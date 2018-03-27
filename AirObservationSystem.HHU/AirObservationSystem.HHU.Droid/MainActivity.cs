using AirObservationSystem.HHU.Core;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Views.Pages;
using Xamarin.Forms.Platform.Android;
using Android.OS;
using Android.App;
using Android.Content.PM;
using Autofac;
using Plugin.Toasts;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Droid
{
    [Activity(Label = "AirObservationSystem.HHU.Droid", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        public static IContainer Container { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //var builder = new ContainerBuilder();

            //builder.RegisterInstance(new DroidPlatform()).As<IPlatform>();
            //builder.RegisterType<MainViewModel>();

            //Container = builder.Build();

            var setup = new Setup();
            AppContainer.Container = setup.CreateContainer();

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            DependencyService.Register<ToastNotification>();
            ToastNotification.Init(this, new PlatformOptions() { SmallIconDrawable = Android.Resource.Drawable.IcDialogInfo });

            LoadApplication(new App(typeof(CommunicationSelection)));
        }
    }
}