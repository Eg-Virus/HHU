using System;
using System.Collections.Generic;
using System.Linq;
using AirObservationSystem.HHU.Core.Helpers;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class ObservationMessageSummary
    {
        public ObservationMessageSummary()
        {
            InitializeComponent();
            AfterLocalInitialize();

        }

        public override void InitializeLocalProperites()
        {
            base.InitializeLocalProperites();

            LblPageName.Text = Resx.Resources.lblTextMessageSummaryPageName;
            BtnBack.Image = Static.GetImageSource("back.png") as FileImageSource;
            BtnBack.Text = Resx.Resources.BtnBack;
            BtnSend.Text = Resx.Resources.BtnSend;
            BtnSend.Image = Static.GetImageSource("send.png") as FileImageSource;

            //TODO:Apply localization here
            LblObservationType.Text = Static.ObservationType.ToString();
            LblObservationNumber.Text = Static.ObservationNumber.ToString();
            LblAirborneType.Text = Static.AirborneType.ToString();
            LblAltitude.Text = Static.Altitude.ToString();
            LblHeading.Text = Static.Heading.ToString();
        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();

            PageInfoGridVisible = false;
            ProgressBarGridVisible = true;
            ProgressBarRefreshVisable = false;
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            //new thread
            Device.BeginInvokeOnMainThread(async () =>
            {

                //if (await DisplayAlert(Resx.Resources.ChangeConnectionTitle, Resx.Resources.ChangeConnectionMessage, Resx.Resources.Yes, Resx.Resources.No))
                {
                    //other methods
                    await Navigation.PopAsync();
                }

            });

            return true; //Do not navigate backwards by pressing the button
        }

        private void BtnBack_OnClicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private async void BtnSend_OnClicked(object sender, EventArgs e)
        {
            var pList = new List<object>
            {
                (byte) Static.ObservationNumber,
                (byte) Static.ObservationType,
                (byte) Static.Altitude,
                (byte) Static.Heading,
                DateTime.Now
            };

            await ViewModel.SendAsync(pList,
                Static.ObservationType == Helpers.ObservationType.Audio
                    ? MessageType.AudibleObservation
                    : MessageType.VisualObservation);

            var lst =
                Navigation.NavigationStack.Where(
                    p =>
                        p.GetType() != typeof(Login) && p.GetType() != typeof(CommunicationSelection) &&
                        p.GetType() != typeof(ObservationType) && p.GetType() != typeof(ObservationMessageSummary)).ToList();
            foreach (var p in lst)
                Navigation.RemovePage(p);

            await Navigation.PopAsync();

        }
    }
}
