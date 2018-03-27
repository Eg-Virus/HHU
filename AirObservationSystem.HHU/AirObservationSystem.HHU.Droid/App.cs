using System;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.Core.ViewModels;
using AirObservationSystem.HHU.Droid.Implementations;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Autofac;

namespace AirObservationSystem.HHU.Droid
{
    //[Activity(
    //    Label = "AirObservationSystem.HHU.Droid"
    //    , Icon = "@drawable/icon"
    //    , ScreenOrientation = ScreenOrientation.Portrait)]
    //public class App : Application
    //{
    //    public static IContainer Container { get; set; }

    //    public App(IntPtr h, JniHandleOwnership jho) : base(h, jho)
    //    {
    //    }

    //    public override void OnCreate()
    //    {
    //        var builder = new ContainerBuilder();

    //        builder.RegisterInstance(new DroidPlatform()).As<IPlatform>();
    //        builder.RegisterType<MainViewModel>();

    //        Container = builder.Build();

    //        base.OnCreate();
    //    }
    //}
    //public class SplashScreen
    //    //: MvxSplashScreenActivity
    //{
    //    //public SplashScreen()
    //    //    : base(Resource.Layout.SplashScreen)
    //    //{
    //    //}

    //    //private bool isInitializationComplete = false;
    //    //public override void InitializationComplete()
    //    //{
    //    //    if (!isInitializationComplete)
    //    //    {
    //    //        isInitializationComplete = true;
    //    //        StartActivity(typeof(MvxFormsApplicationActivity));
    //    //    }
    //    //}

    //    //protected override void OnCreate(Android.OS.Bundle bundle)
    //    //{
    //    //    Forms.Init(this, bundle);
    //    //    // Leverage controls' StyleId attrib. to Xamarin.UITest
    //    //    Forms.ViewInitialized += (object sender, ViewInitializedEventArgs e) =>
    //    //    {
    //    //        if (!string.IsNullOrWhiteSpace(e.View.StyleId))
    //    //        {
    //    //            e.NativeView.ContentDescription = e.View.StyleId;
    //    //        }
    //    //    };

    //    //    base.OnCreate(bundle);
    //    //}
    //}
}