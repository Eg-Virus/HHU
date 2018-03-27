using System;
using AirObservationSystem.HHU.Core.Helpers;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class ObservationNumber 
    {
        public ObservationNumber()
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
            Static.ObservationNumber = Helpers.NumberofTargets.Other;

            BtnNext.Image = Static.GetImageSource("next.png") as FileImageSource;
            BtnBack.Image = Static.GetImageSource("back.png") as FileImageSource;
            BtnNext.Text = Resx.Resources.BtnNext;
            BtnBack.Text = Resx.Resources.BtnBack;

            BtnOne.Image=Static.GetImageSource("one.png") as FileImageSource;
            BtnTwo.Image=Static.GetImageSource("two.png") as FileImageSource;
            BtnThreeOrMore.Image=Static.GetImageSource("three.png") as FileImageSource;
            LblOne.Text = Resx.Resources.LblOne;
            LblTwo.Text = Resx.Resources.LblTwo;
            LblThreeOrMore.Text = Resx.Resources.LblThreeOrMore;
        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();
            PageName = Resx.Resources.LblObservationNumberPageName;
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

        private void BtnOne_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            BtnOne.BorderColor = Color.White;
            BtnTwo.BorderColor=Color.Transparent;
            BtnThreeOrMore.BorderColor=Color.Transparent;

            Static.ObservationNumber = Helpers.NumberofTargets.One;
        }

        private void BtnTwo_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            BtnOne.BorderColor = Color.Transparent;
            BtnTwo.BorderColor = Color.White;
            BtnThreeOrMore.BorderColor = Color.Transparent;

            Static.ObservationNumber = Helpers.NumberofTargets.Two;
        }

        private void BtnThreeOrMore_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            BtnOne.BorderColor = Color.Transparent;
            BtnTwo.BorderColor = Color.Transparent;
            BtnThreeOrMore.BorderColor = Color.White;

            Static.ObservationNumber = Helpers.NumberofTargets.Many;
        }

        private void BtnBack_OnClicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private async void BtnNext_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AirborneCraft(), true);
        }
    }
}
