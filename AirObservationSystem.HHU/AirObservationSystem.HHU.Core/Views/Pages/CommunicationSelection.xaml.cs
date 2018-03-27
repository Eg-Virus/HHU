using System;
using System.Threading.Tasks;
using AirObservationSystem.HHU.Core.Helpers;
using AirObservationSystem.HHU.Core.Model.Old_Code;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class CommunicationSelection
    {
        public CommunicationSelection()
        {
            InitializeComponent();

            AfterLocalInitialize();

        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();

            CommunicationType = null;
            CommunicationTypeIsVisible = false;

            StationName = "";

            PageName = Resx.Resources.LblCommunicationSelectionPageName;
            PageImage = Static.GetImageSource("comminucation.png");
            ProgressBarGridVisible = false;
        }

        public override void InitializeLocalProperites()
        {
            base.InitializeLocalProperites();

            LblSerial.Text = Resx.Resources.LblSerial;
            LblBluetooth.Text = Resx.Resources.LblBluetooth;

            BtnSerial.Image = Static.GetImageSource("serial.png") as FileImageSource;
            BtnBluetooth.Image = Static.GetImageSource("bluetooth.png") as FileImageSource;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AfterLocalInitialize();
            
            StopProgress = true;
        }

        private async void BtnSerial_OnClicked(object sender, EventArgs e)
        {
            await SetProperties(Helpers.CommunicationType.Serial);
        }

        private async void BtnBluetooth_OnClicked(object sender, EventArgs e)
        {
            await SetProperties(Helpers.CommunicationType.Bluetooth);
        }

        private async Task SetProperties(CommunicationType communicationType)
        {
            CommunicationTypeIsVisible = true;

            Static.CurrentCommunicationType = communicationType;
            ViewModel.SelectMedia();

            if (await ViewModel.InitializeMediaAsync() == Result.Success)
            {
                switch (communicationType)
                {
                    case Helpers.CommunicationType.Serial:
                        CommunicationType = Static.GetImageSource("serial_icon.png") as FileImageSource;
                        break;
                    case Helpers.CommunicationType.Bluetooth:
                       CommunicationType = Static.GetImageSource("bluetooth_icon.png") as FileImageSource;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(communicationType), communicationType, null);
                }

                //Check if there's any changes in the FIU before login and wait until FIU detect the Connection
                await ViewModel.SendAsync(ViewModel.StatusRequest(StatusRequestType.ConnectionStateChange, RequestPacketTypes.Initial, 0),
                                          MessageType.MessagesStatusRequest);
                //Redirect to next page
                await Navigation.PushAsync(new Login(), true);
            }
            else
            {
                await DisplayAlert(Resx.Resources.ConnectionFailedTitle, Resx.Resources.ConnectionFailedText, Resx.Resources.BtnOk);
                //await Static.ShowToast(new NotificationOptions()
                //{
                //    Title = "Connection Failed",
                //    Description = "Couldn't find the Device",
                //    IsClickable = true,
                //    //WindowsOptions = new WindowsOptions() { LogoUri = "icon.png" },
                //    ClearFromHistory = false
                //});
            }
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            //new thread
            //Device.BeginInvokeOnMainThread(async () =>
            //{

            //    var result = await DisplayAlert(Resx.Resources.ExitTitle, Resx.Resources.ExitMessage, Resx.Resources.Yes, Resx.Resources.No);

            //    if (result)
            //    {
            //        //other methods
            //        //DependencyService.Get<IPlatform>().CloseApp();
            //    }

            //});

            return true; //Do not navigate backwards by pressing the button
        }
    }
}
