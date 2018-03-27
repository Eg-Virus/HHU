using System;
using AirObservationSystem.HHU.Core.Helpers;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class TextMessageType 
    {
        public TextMessageType()
        {
            InitializeComponent();

            AfterLocalInitialize();
        }

        public override void OnLanguageChaged(string language)
        {
            base.OnLanguageChaged(language);


            if (language == "en")
            {
                Grid.SetColumn(BtnBack, 0);
            }
            else
            {
                Grid.SetColumn(BtnBack, 2);
            }
        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();

            PageName = Resx.Resources.lblTextMessageTypePageName;
            PageImage = Static.GetImageSource("obsarve.png");
            PageInfoGridVisible = true;
            ProgressBarGridVisible = true;
            ProgressBarRefreshVisable = false;
        }

        public override void InitializeLocalProperites()
        {
            base.InitializeLocalProperites();

            BtnOperationBase.Image = Static.GetImageSource("operation_base.png") as FileImageSource;
            BtnObservationBase.Image = Static.GetImageSource("observation_base.png") as FileImageSource;
            LblOperationBase.Text = Resx.Resources.LblOperationBase;
            LblObservationBase.Text = Resx.Resources.LblObservationBase;

            BtnBack.Text = Resx.Resources.BtnBack;
            BtnBack.Image = Static.GetImageSource("back.png") as FileImageSource;

        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            //new thread
            Device.BeginInvokeOnMainThread(async () =>
            {
                //var result = await DisplayAlert(Resx.Resources.LogoutTitle, Resx.Resources.LogoutMessage, Resx.Resources.Yes, Resx.Resources.No);
                //if (await DisplayAlert(Resx.Resources.LogoutTitle, Resx.Resources.LogoutMessage, Resx.Resources.Yes, Resx.Resources.No))
                {
                    await Navigation.PopAsync();
                }
            });
            return true; //Do not navigate backwards by pressing the button
        }

        private void BtnBack_OnClicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private async void BtnOperationBase_OnClicked(object sender, EventArgs e)
        {
            Static.MessageDirection=MessageDirection.Operation;
            await Navigation.PushAsync(new ComposeMessage(), true);
        }

        private async void BtnObservationBase_OnClicked(object sender, EventArgs e)
        {
            Static.MessageDirection = MessageDirection.Observation;
            await Navigation.PushAsync(new ComposeMessage(), true);
        }
    }
}
