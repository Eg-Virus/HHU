using System;
using AirObservationSystem.HHU.Core.Helpers;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class AirborneCraft 
    {
        public AirborneCraft()
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
            Static.AirborneType = TargetType.Other;

            BtnNext.Image = Static.GetImageSource("next.png") as FileImageSource;
            BtnBack.Image = Static.GetImageSource("back.png") as FileImageSource;
            BtnNext.Text = Resx.Resources.BtnNext;
            BtnBack.Text = Resx.Resources.BtnBack;

            BtnDrone.Image = Static.GetImageSource("umaned_aerial_drone.png") as FileImageSource;
            BtnFighter.Image = Static.GetImageSource("fighter.png") as FileImageSource;
            BtnMilitary.Image = Static.GetImageSource("military_plane.png") as FileImageSource;
            //BtnUnidentified.Image = Static.GetImageSource("unidentified_flying_object.png") as FileImageSource;
            BtnCivilian.Image = Static.GetImageSource("civilian_airplane.png") as FileImageSource;
            BtnHelicopter.Image = Static.GetImageSource("helicopter.png") as FileImageSource;
            LblDrone.Text = Resx.Resources.LblDrone;
            LblFighter.Text = Resx.Resources.LblFighter;
            LblMilitary.Text = Resx.Resources.LblMilitary;
            //LblUnidentified.Text = Resx.Resources.LblUnidentified;
            LblCivilian.Text = Resx.Resources.LblCivilian;
            LblHelicopter.Text = Resx.Resources.LblHelicopter;
        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();
            PageName = Resx.Resources.LblAirborneCraftPageName;
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

        private void BtnFighter_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.AirborneType = TargetType.FighterJet;

            BtnFighter.BorderColor=Color.White;
            BtnHelicopter.BorderColor = Color.Transparent;
            //BtnUnidentified.BorderColor=Color.Transparent;
            BtnDrone.BorderColor=Color.Transparent;
            BtnCivilian.BorderColor=Color.Transparent;
            BtnMilitary.BorderColor=Color.Transparent;
        }

        private void BtHelicopter_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.AirborneType = TargetType.Helicopter;

            BtnFighter.BorderColor = Color.Transparent;
            BtnHelicopter.BorderColor = Color.White;
            //BtnUnidentified.BorderColor = Color.Transparent;
            BtnDrone.BorderColor = Color.Transparent;
            BtnCivilian.BorderColor = Color.Transparent;
            BtnMilitary.BorderColor = Color.Transparent;
        }

        private void BtnDrone_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.AirborneType = TargetType.UAV;

            BtnFighter.BorderColor = Color.Transparent;
            BtnHelicopter.BorderColor = Color.Transparent;
            //BtnUnidentified.BorderColor = Color.Transparent;
            BtnDrone.BorderColor = Color.White;
            BtnCivilian.BorderColor = Color.Transparent;
            BtnMilitary.BorderColor = Color.Transparent;
        }

        private void BtnUnidentified_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.AirborneType = TargetType.Other;
            
            BtnFighter.BorderColor = Color.Transparent;
            BtnHelicopter.BorderColor = Color.Transparent;
            //BtnUnidentified.BorderColor = Color.White;
            BtnDrone.BorderColor = Color.Transparent;
            BtnCivilian.BorderColor = Color.Transparent;
            BtnMilitary.BorderColor = Color.Transparent;
        }

        private void BtCivilian_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.AirborneType = TargetType.CivilTrans;

            BtnFighter.BorderColor = Color.Transparent;
            BtnHelicopter.BorderColor = Color.Transparent;
            //BtnUnidentified.BorderColor = Color.Transparent;
            BtnDrone.BorderColor = Color.Transparent;
            BtnCivilian.BorderColor = Color.White;
            BtnMilitary.BorderColor = Color.Transparent;
        }

        private void BtnMilitary_OnClicked(object sender, EventArgs e)
        {
            ProgressBarProgress = 0.0;

            Static.AirborneType = TargetType.MilitaryTrans;

            BtnFighter.BorderColor = Color.Transparent;
            BtnHelicopter.BorderColor = Color.Transparent;
            //BtnUnidentified.BorderColor = Color.Transparent;
            BtnDrone.BorderColor = Color.Transparent;
            BtnCivilian.BorderColor = Color.Transparent;
            BtnMilitary.BorderColor = Color.White;
        }

        private void BtnBack_OnClicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private async void BtnNext_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Altitude(), true);
        }
    }
}
