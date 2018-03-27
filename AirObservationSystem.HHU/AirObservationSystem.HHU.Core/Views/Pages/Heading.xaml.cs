using System;
using AirObservationSystem.HHU.Core.Helpers;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class Heading
    {
        public Heading()
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
            Static.Heading = Helpers.Heading.Other;

            BtnNext.Image = Static.GetImageSource("next.png") as FileImageSource;
            BtnBack.Image = Static.GetImageSource("back.png") as FileImageSource;
            BtnNext.Text = Resx.Resources.BtnNext;
            BtnBack.Text = Resx.Resources.BtnBack;

            BtnNorth.Image = Static.GetImageSource("north.png") as FileImageSource;
            BtnEast.Image = Static.GetImageSource("east.png") as FileImageSource;
            BtnWest.Image = Static.GetImageSource("west.png") as FileImageSource;
            BtnSouth.Image = Static.GetImageSource("south.png") as FileImageSource;
            LblNorth.Text = Resx.Resources.LblNorth;
            LblEast.Text = Resx.Resources.LblEast;
            LblWest.Text = Resx.Resources.LblWest;
            LblSouth.Text = Resx.Resources.LblSouth;
        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();
            PageName = Resx.Resources.LblHeadingPageName;
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

        private void BtnNorth_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.Heading = Helpers.Heading.SN;

            BtnNorth.BorderColor = Color.White;
            BtnEast.BorderColor = Color.Transparent;
            BtnWest.BorderColor = Color.Transparent;
            BtnSouth.BorderColor = Color.Transparent;
        }

        private void BtEast_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.Heading = Helpers.Heading.WE;

            BtnNorth.BorderColor = Color.Transparent;
            BtnEast.BorderColor = Color.White;
            BtnWest.BorderColor = Color.Transparent;
            BtnSouth.BorderColor = Color.Transparent;
        }

        private void BtnWest_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.Heading = Helpers.Heading.EW;

            BtnNorth.BorderColor = Color.Transparent;
            BtnEast.BorderColor = Color.Transparent;
            BtnWest.BorderColor = Color.White;
            BtnSouth.BorderColor = Color.Transparent;
        }

        private void BtnSouth_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.Heading = Helpers.Heading.NS;

            BtnNorth.BorderColor = Color.Transparent;
            BtnEast.BorderColor = Color.Transparent;
            BtnWest.BorderColor = Color.Transparent;
            BtnSouth.BorderColor = Color.White;
        }

        private void BtnBack_OnClicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private async void BtnNext_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ObservationMessageSummary(), true);
        }
    }
}
