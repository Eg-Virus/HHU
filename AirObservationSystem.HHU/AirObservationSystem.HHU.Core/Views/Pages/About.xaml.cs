using System;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class About 
    {
        public About()
        {
            InitializeComponent();
            AfterLocalInitialize();
        }

        public override void InitializeLocalProperites()
        {
            base.InitializeLocalProperites();

            //StopProgress = true;

            LblPageName.Text = Resx.Resources.LblAboutPageName;
            LblAppName.Text = Resx.Resources.LblAboutAppName;
            LblAuth.Text = Resx.Resources.LblAboutAuth;
            LblAddress.Text = Resx.Resources.LblAboutAddress;
            BtnOk.Text = Resx.Resources.BtnOk;
            ImgAEC.Source = Static.GetImageSource("about_aec.png");
            ImgRSAF.Source = Static.GetImageSource("about_rsaf.png");
        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();

            ProgressBarGridVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AfterLocalInitialize();
        }

        private async void BtnOk_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
