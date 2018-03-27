using System;
using System.Globalization;
using System.Linq;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.Core.ViewModels;
using AirObservationSystem.HHU.Core.Views;
using AirObservationSystem.HHU.Core.Views.Pages;
using Xamarin.Forms;
using AirObservationSystem.HHU.Core.Helpers;
using Plugin.Battery;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.Message;
using AirObservationSystem.HHU.Core.Model.Old_Code;
using Autofac;

namespace AirObservationSystem.HHU.Core
{
    public partial class App //: Application
    {
        //public App(AppSetup setup, MainView<MainViewModel> mainView)
        public App(Type pageType)
        {
            //Need to be loaded from Settings
            Static.ChangeCurrentCultureInfo(new CultureInfo("en"));

            InitializeComponent();

            AppContainer.Container.Resolve<IPlatform>().SetStatusBar(show: false);

            CrossBattery.Current.BatteryChanged += BatteryChanged;

            MainView<MainViewModel> mainView = new CommunicationSelection();

            mainView.Battery = CrossBattery.Current.RemainingChargePercent + "%";

            mainView.LanguageLetter = AppContainer.Container.Resolve<ICultureInfo>().GetCurrentCultureInfo().TwoLetterISOLanguageName == "en" ? "ع" : "E";

            mainView.ViewModel.DataReceivedNotification += ViewModel_DataReceivedNotification;
            mainView.ViewModel.DataReceivedConnectionChanged += ViewModel_DataReceivedConnectionChanged;

            Static.SerialPort = AppContainer.Container.ResolveKeyed<IMedia>("Serial");
            Static.Bluetooth = AppContainer.Container.ResolveKeyed<IMedia>("Bluetooth");

            //Loading the Unconfirmed messages
            Static.SentMessages =
                AppContainer.Container.Resolve<Repository.Message.SentMessageRepository>()
                    .GetManyAsync(m => m.Status != MessageStatus.Received);

            MainPage = new NavigationPage(mainView);
        }

        private void ViewModel_DataReceivedConnectionChanged(object sender, object e)
        {
            var obj = (GSMConnectNotificationBody)e;
            var stationName = string.Empty;
            FileImageSource communicationType = null;
            switch (obj.GsmConnType)
            {
                case GSMConnectionType.None:
                    //Disconnected
                    if (Static.CurrentCommunicationType == CommunicationType.Serial)
                        communicationType = Static.GetImageSource("serial_icon.png") as FileImageSource;
                    else
                        communicationType = Static.GetImageSource("bluetooth_icon.png") as FileImageSource;
                    break;
                case GSMConnectionType.Normal:
                case GSMConnectionType.Thuriya:
                default:
                    {
                        if (Static.CurrentCommunicationType == CommunicationType.Serial)
                            communicationType = Static.GetImageSource("serial_icon_on.png") as FileImageSource;
                        else
                            communicationType = Static.GetImageSource("bluetooth_icon_on.png") as FileImageSource;
                        switch (obj.ActiveCentral)
                        {
                            case ActiveCentral.TK:
                                stationName = "TK";
                                break;
                            case ActiveCentral.TF:
                                stationName = "TF";
                                break;
                            case ActiveCentral.KM:
                                stationName = "KM";
                                break;
                            case ActiveCentral.None:
                            default:
                                {
                                    //Default image
                                    break;
                                }
                        }
                        break;
                    }
            }
            ((MainView<MainViewModel>)((NavigationPage)Current.MainPage).CurrentPage).CommunicationType = communicationType;
            ((MainView<MainViewModel>)((NavigationPage)Current.MainPage).CurrentPage).StationName = stationName;
        }

        private void ViewModel_DataReceivedNotification(object sender, object e)
        {
            var currentPage = (MainView<MainViewModel>)((NavigationPage)Current.MainPage).CurrentPage;
            switch ((MsgType)e)
            {
                case MsgType.MissedCall:
                    currentPage.Notification = Static.GetImageSource("missed_call.png");
                    currentPage.NotificationIsVisible = true;
                    break;
                case MsgType.MissedSMS:
                case MsgType.HHUMsg:
                case MsgType.Guidance:
                case MsgType.PlatoonMsg:
                    currentPage.Notification = Static.GetImageSource("missed_msg.png");
                    currentPage.NotificationIsVisible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }

        private void BatteryChanged(object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs e)
        {
            ((MainView<MainViewModel>)((NavigationPage)Current.MainPage).CurrentPage).Battery =
                e.RemainingChargePercent + "%";
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            ((MainView<MainViewModel>)((NavigationPage)Current.MainPage).CurrentPage).Battery =
                CrossBattery.Current.RemainingChargePercent + "%";
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void BtnLang_OnClicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            var lang = btn.Text == "E" ? "en" : "ar";

            Static.ChangeCurrentCultureInfo(new CultureInfo(lang));
            ((MainView<MainViewModel>)((NavigationPage)Current.MainPage).CurrentPage).Language = lang;
            //btn.Text = btn.Text == "E" ? "ع" : "E";
        }

        private void BtnRefresh_OnClicked(object sender, EventArgs e)
        {
            ((MainView<MainViewModel>)((NavigationPage)Current.MainPage).CurrentPage).ProgressBarProgress = 0;
        }

        private async void BtnCommunicationType_OnClicked(object sender, EventArgs e)
        {
            var currentPage = ((MainView<MainViewModel>)((NavigationPage)Current.MainPage).CurrentPage);
            if (await currentPage.DisplayAlert(Resx.Resources.ChangeConnectionTitle, Resx.Resources.ChangeConnectionMessage, Resx.Resources.Yes,
                Resx.Resources.No))
            {
                Static.KeepAlive = false;
                currentPage.CommunicationTypeIsVisible = false;
                await currentPage.Navigation.PopToRootAsync();
            }
        }

        private async void BtnAbout_OnClicked(object sender, EventArgs e)
        {
            var currentPage = (MainView<MainViewModel>)((NavigationPage)Current.MainPage).CurrentPage;
            var pages = currentPage.Navigation.NavigationStack.Where(p => p.GetType() == typeof(About)).ToList();
            if (pages.Count > 0)
                currentPage.Navigation.RemovePage(pages[0]);

            await currentPage.Navigation.PushAsync(new About(), true);

        }

        private async void BtnNotification_OnClicked(object sender, EventArgs e)
        {
            var currentPage = ((MainView<MainViewModel>)((NavigationPage)Current.MainPage).CurrentPage);
            var missedMessages = Static.UnReadMessages.Where(m => m.Type != MsgType.MissedCall).ToList();
            var missedCalles = Static.UnReadMessages.Where(m => m.Type == MsgType.MissedCall).ToList();
            if (missedMessages.Count > 0)
            {
                if (Static.CurrentUser.IsLogedIn)
                {

                    if (missedMessages.Count > 1)
                    {
                        Static.UnReadMessages.RemoveAll(m => m.Type != MsgType.MissedCall);
                        await currentPage.Navigation.PushAsync(new Reports(), true);
                    }
                    else
                    {
                        await currentPage.DisplayAlert("Client Message", missedMessages[0].SmsContent, Resx.Resources.BtnOk);
                        Static.UnReadMessages.RemoveAll(m => m.Type != MsgType.MissedCall);
                    }
                }
            }
            else if (missedCalles.Count > 0) //TODO: Add the Missed calls in the DB and show it in the reports
            {


            }
            currentPage.NotificationIsVisible = false;
        }
    }
}
