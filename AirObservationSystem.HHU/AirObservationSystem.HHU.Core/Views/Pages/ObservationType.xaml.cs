using System;
using System.Collections.Generic;
using System.Linq;
using AirObservationSystem.HHU.Core.Helpers;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class ObservationType
    {
        //private bool _stopProgress = false;
        public ObservationType()
        {
            InitializeComponent();

            //AfterLocalInitialize();

            //ViewModel.DataReceivedDestinationCall += ViewModel_DataReceivedDestinationCall;
        }

        private void ViewModel_DataReceivedDestinationCall(object sender, object e)
        {
            PkrDestinationCall.SelectedIndex = int.Parse(((byte) Static.DestinationCall).ToString());
        }

        public override void OnLanguageChaged(string language)
        {
            base.OnLanguageChaged(language);


            if (language == "en")
            {
            }
            else
            {
            }
        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();

            PageName = Resx.Resources.LblObservationTypePageName;
            PageImage = Static.GetImageSource("obsarve.png");
            PageInfoGridVisible = true;
            ProgressBarGridVisible = true;
            ProgressBarRefreshVisable = true;
            ProgressBarRefreshImage= Static.GetImageSource("refresh.png") as FileImageSource;
        }

        public override void InitializeLocalProperites()
        {
            base.InitializeLocalProperites();

            BtnText.Image = Static.GetImageSource("text.png") as FileImageSource;
            BtnAudible.Image = Static.GetImageSource("audio.png") as FileImageSource;
            BtnVisual.Image = Static.GetImageSource("visual.png") as FileImageSource;
            LblText.Text = Resx.Resources.LblText;
            LblAudible.Text = Resx.Resources.LblAudible;
            LblVisual.Text = Resx.Resources.LblVisual;

            PkrDestinationCall.Items.Clear();
            foreach (var k in Enum.GetNames(typeof(DestinationCall)))
                PkrDestinationCall.Items.Add(k);
            PkrDestinationCall.SelectedIndex = int.Parse(((byte)Static.DestinationCall).ToString());

            BtnLogout.Image = Static.GetImageSource("logout.png") as FileImageSource;
            BtnReports.Image = Static.GetImageSource("reports.png") as FileImageSource;
            BtnLogout.Text = Resx.Resources.BtnLogout;
            BtnReports.Text = Resx.Resources.BtnReports;

        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            //new thread
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert(Resx.Resources.LogoutTitle, Resx.Resources.LogoutMessage, Resx.Resources.Yes, Resx.Resources.No))
                {
                    await Navigation.PopAsync();
                }
            });
            return true; //Do not navigate backwards by pressing the button
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AfterLocalInitialize();

            ViewModel.DataReceivedDestinationCall += ViewModel_DataReceivedDestinationCall;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ViewModel.DataReceivedDestinationCall -= ViewModel_DataReceivedDestinationCall;
        }

        private void BtnLogout_OnClicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private async void BtnText_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;
            BtnText.BorderColor = Color.White;
            BtnAudible.BorderColor = Color.Transparent;
            BtnVisual.BorderColor = Color.Transparent;
            Static.ObservationType = Helpers.ObservationType.Text;

            await Navigation.PushAsync(new TextMessageType(), true);
        }

        private async void BtnAudible_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;
            BtnText.BorderColor = Color.Transparent;
            BtnAudible.BorderColor = Color.White;
            BtnVisual.BorderColor = Color.Transparent;
            Static.ObservationType = Helpers.ObservationType.Audio;

            await Navigation.PushAsync(new ObservationNumber(), true);
        }

        private async void BtnVisual_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;
            BtnText.BorderColor = Color.Transparent;
            BtnAudible.BorderColor = Color.Transparent;
            BtnVisual.BorderColor = Color.White;
            Static.ObservationType = Helpers.ObservationType.Video;

            await Navigation.PushAsync(new ObservationNumber(), true);
        }

        private async void BtnReports_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Reports(), true);
        }

        private async void PkrDestinationCall_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Static.DestinationCall = (DestinationCall) PkrDestinationCall.SelectedIndex;

            await ViewModel.SendAsync(null, MessageType.CallDestination);
        }
    }
}
