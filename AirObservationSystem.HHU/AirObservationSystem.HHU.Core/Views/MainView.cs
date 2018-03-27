using System;
using AirObservationSystem.HHU.Core.Helpers;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.ViewModels;
using Autofac;
using Plugin.Battery;
using Xamarin.Forms;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.Core.Views.Pages;

namespace AirObservationSystem.HHU.Core.Views
{
    public class MainView<T> : BaseView<MainViewModel>
    {
        public T ViewModel { get; }

        public bool StopProgress = false;

        public MainView()
        {
            using (var scope = AppContainer.Container.BeginLifetimeScope())
            {
                ViewModel = AppContainer.Container.Resolve<T>();
            }
            BindingContext = ViewModel;
            PropertyChanged += MainView_PropertyChanged;
            Device.StartTimer(TimeSpan.FromSeconds(1), ProgressBarIncremental);

            NavigationPage.SetHasNavigationBar(this, false);
        }

        internal void AfterLocalInitialize()
        {
            InitializeBindableProperties();

            InitializeLocalProperites();

            OnLanguageChaged(AppContainer.Container.Resolve<ICultureInfo>().GetCurrentCultureInfo().TwoLetterISOLanguageName);
        }

        private bool ProgressBarIncremental()
        {
            if (ProgressBarProgress < 1 && !StopProgress)
            {
                ProgressBarProgress += (1 / Static.ProgressBarMax);
                return true;
            }
            if (ProgressBarProgress >= 1) //PopPagesAsync
                Navigation.PushAsync(new Lock(), true);

            return false;
        }

        private void MainView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Language")
            {
                OnLanguageChaged(Language);
                InitializeBindableProperties();
                InitializeLocalProperites();
            }
            if (e.PropertyName == "Refresh")
            {
                ProgressBarProgress = 0;
            }
        }

        public virtual void InitializeLocalProperites()
        { }

        public virtual void InitializeBindableProperties()
        {
            CommunicationTypeIsVisible = Static.CommunicationTypeIsVisible;
            CommunicationType = Static.CommunicationType as FileImageSource;
            StationName = Static.StationName;
            LanguageLetter = AppContainer.Container.Resolve<ICultureInfo>().GetCurrentCultureInfo().TwoLetterISOLanguageName == "en" ? "ع" : "E";
            ProgressBarProgress = 0.0;
            StopProgress = false;
        }

        public virtual void OnLanguageChaged(string language)
        {
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (StopProgress)
            {
                StopProgress = false;
                Device.StartTimer(TimeSpan.FromSeconds(1), ProgressBarIncremental);
            }
            ProgressBarProgress = 0.0;
            CommunicationTypeIsVisible = Static.CommunicationTypeIsVisible;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        #region Bindable Properties

        private string _language;
        public string Language
        {
            get { return _language; }
            set
            {
                _language = value;
                OnPropertyChanged();
            }
        }

        public static readonly BindableProperty BindablePageName =
            BindableProperty.Create("PageName", typeof(string), typeof(T));
        public string PageName
        {
            get { return (string)GetValue(BindablePageName); }
            set { SetValue(BindablePageName, value); }
        }

        public static readonly BindableProperty BindablePageImage =
            BindableProperty.Create("PageImage", typeof(ImageSource), typeof(T));
        public ImageSource PageImage
        {
            get { return (ImageSource)GetValue(BindablePageImage); }
            set { SetValue(BindablePageImage, value); OnPropertyChanged(); }
        }

        //public static readonly BindableProperty BindableCommunicationType =
        //    BindableProperty.Create("CommunicationType", typeof(ImageSource), typeof(T));
        //public ImageSource CommunicationType
        //{
        //    get { return (ImageSource)GetValue(BindableCommunicationType); }
        //    set
        //    {
        //        SetValue(BindableCommunicationType, value);
        //        Static.CommunicationType = value;
        //        OnPropertyChanged();
        //    }
        //}

        public static readonly BindableProperty BindableCommunicationTypeIsVisible =
            BindableProperty.Create("CommunicationTypeIsVisible", typeof(bool), typeof(T), false);
        public bool CommunicationTypeIsVisible
        {
            get { return (bool)GetValue(BindableCommunicationTypeIsVisible); }
            set
            {
                SetValue(BindableCommunicationTypeIsVisible, value);
                Static.CommunicationTypeIsVisible = value;
                OnPropertyChanged();
            }
        }

        public static readonly BindableProperty BindableCommunicationType =
            BindableProperty.Create("CommunicationType", typeof(FileImageSource), typeof(T));
        public FileImageSource CommunicationType
        {
            get { return (FileImageSource)GetValue(BindableCommunicationType); }
            set
            {
                SetValue(BindableCommunicationType, value);
                Static.CommunicationType = value;
                OnPropertyChanged();
            }
        }

        public static readonly BindableProperty BindableStationName =
            BindableProperty.Create("StationName", typeof(string), typeof(T));
        public string StationName
        {
            get { return (string)GetValue(BindableStationName); }
            set
            {
                SetValue(BindableStationName, value);
                Static.StationName = value;
                OnPropertyChanged();
            }
        }

        public static readonly BindableProperty BindablePageInfoGridVisible =
            BindableProperty.Create("PageInfoGridVisible", typeof(bool), typeof(T), true);
        public bool PageInfoGridVisible
        {
            get { return (bool)GetValue(BindablePageInfoGridVisible); }
            set { SetValue(BindablePageInfoGridVisible, value); OnPropertyChanged(); }
        }

        public static readonly BindableProperty BindableProgressBarGridVisible =
            BindableProperty.Create("ProgressBarGridVisible", typeof(bool), typeof(T), true);
        public bool ProgressBarGridVisible
        {
            get { return (bool)GetValue(BindableProgressBarGridVisible); }
            set { SetValue(BindableProgressBarGridVisible, value); OnPropertyChanged(); }
        }
        public static readonly BindableProperty BindableProgressBarRefreshImage =
            BindableProperty.Create("ProgressBarRefreshImage", typeof(FileImageSource), typeof(T), null);
        public FileImageSource ProgressBarRefreshImage
        {
            get { return (FileImageSource)GetValue(BindableProgressBarRefreshImage); }
            set { SetValue(BindableProgressBarRefreshImage, value); OnPropertyChanged(); }
        }
        
        public static readonly BindableProperty BindableProgressBarProgress =
            BindableProperty.Create("ProgressBarProgress", typeof(double), typeof(T), 0.0);
        public double ProgressBarProgress
        {
            get { return (double)GetValue(BindableProgressBarProgress); }
            set
            {
                SetValue(BindableProgressBarProgress, value);
                OnPropertyChanged();
            }
        }
        public static readonly BindableProperty BindableProgressBarRefreshVisable =
            BindableProperty.Create("ProgressBarRefreshVisable", typeof(bool), typeof(T), false);
        public bool ProgressBarRefreshVisable
        {
            get { return (bool)GetValue(BindableProgressBarRefreshVisable); }
            set { SetValue(BindableProgressBarRefreshVisable, value); OnPropertyChanged(); }
        }

        public static readonly BindableProperty BindableBattery =
            BindableProperty.Create("Battery", typeof(string), typeof(T), CrossBattery.Current.RemainingChargePercent.ToString() + "%");
        public string Battery
        {
            get { return (string)GetValue(BindableBattery); }
            set { SetValue(BindableBattery, value); OnPropertyChanged(); }
        }

        public static readonly BindableProperty BindableLanguageLetter =
            BindableProperty.Create("LanguageLetter", typeof(string), typeof(T), AppContainer.Container.Resolve<ICultureInfo>().GetCurrentCultureInfo().TwoLetterISOLanguageName == "en" ? "ع" : "E");
        public string LanguageLetter
        {
            get { return (string)GetValue(BindableLanguageLetter); }
            set { SetValue(BindableLanguageLetter, value); OnPropertyChanged(); }
        }

        public static readonly BindableProperty BindableNotification =
            BindableProperty.Create("Notification", typeof(ImageSource), typeof(T));
        public ImageSource Notification
        {
            get { return (ImageSource)GetValue(BindableNotification); }
            set { SetValue(BindableNotification, value); OnPropertyChanged(); }
        }

        public static readonly BindableProperty BindableNotificationIsVisible =
            BindableProperty.Create("NotificationIsVisible", typeof(bool), typeof(T), false);
        public bool NotificationIsVisible
        {
            get { return (bool)GetValue(BindableNotificationIsVisible); }
            set { SetValue(BindableNotificationIsVisible, value); OnPropertyChanged(); }
        }

        #endregion
    }
}
