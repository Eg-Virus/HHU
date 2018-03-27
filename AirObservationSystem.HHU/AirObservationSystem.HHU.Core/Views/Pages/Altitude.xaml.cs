using System;
using AirObservationSystem.HHU.Core.Helpers;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class Altitude 
    {
        public Altitude()
        {
            InitializeComponent();

            AfterLocalInitialize();
        }

        public override void OnLanguageChaged(string language)
        {
            base.OnLanguageChaged(language);
            if (language == "ar")
            {
            }
            else
            {
            }

        }

        public override void InitializeLocalProperites()
        {
            base.InitializeLocalProperites();

            //Set Defualt
            Static.Altitude = Helpers.Altitude.Other;


            BtnNext.Image = Static.GetImageSource("next.png") as FileImageSource;
            BtnBack.Image = Static.GetImageSource("back.png") as FileImageSource;
            BtnNext.Text = Resx.Resources.BtnNext;
            BtnBack.Text = Resx.Resources.BtnBack;

            BtnHigh.Image = Static.GetImageSource("high.png") as FileImageSource;
            BtnMid.Image = Static.GetImageSource("mid.png") as FileImageSource;
            BtnLow.Image = Static.GetImageSource("low.png") as FileImageSource;
            LblLow.Text = Resx.Resources.LblLow;
            LblMid.Text = Resx.Resources.LblMid;
            LblHigh.Text = Resx.Resources.LblHigh;
        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();
            PageName = Resx.Resources.LblAltitudePageName;
            PageImage = Static.GetImageSource("obsarve.png");
            PageInfoGridVisible = true;
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

        private async void BtnNext_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Heading(), true);
        }

        private void BtnHigh_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.Altitude = Helpers.Altitude.High;

            BtnHigh.BorderColor=Color.White;
            BtnMid.BorderColor=Color.Transparent;
            BtnLow.BorderColor=Color.Transparent;
        }

        private void BtnMid_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.Altitude = Helpers.Altitude.Mid;

            BtnHigh.BorderColor = Color.Transparent;
            BtnMid.BorderColor = Color.White;
            BtnLow.BorderColor = Color.Transparent;
        }

        private void BtnLow_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.Altitude = Helpers.Altitude.Low;

            BtnHigh.BorderColor = Color.Transparent;
            BtnMid.BorderColor = Color.Transparent;
            BtnLow.BorderColor = Color.White;
        }
    }
}
