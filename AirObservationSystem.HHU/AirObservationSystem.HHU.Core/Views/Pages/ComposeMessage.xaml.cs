using System;
using AirObservationSystem.HHU.Core.Helpers;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class ComposeMessage
    {
        public ComposeMessage()
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

            LblPageName.Text = Resx.Resources.LblComposeMsgPageName;
            BtnNext.Image = Static.GetImageSource("next.png") as FileImageSource;
            BtnBack.Image = Static.GetImageSource("back.png") as FileImageSource;
            BtnNext.Text = Resx.Resources.BtnNext;
            BtnBack.Text = Resx.Resources.BtnBack;
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

        private async void BtnNext_OnClicked(object sender, EventArgs e)
        {
            Static.Message = TxtMsge.Text;
            await Navigation.PushAsync(new TextMessageSummary(), true);
        }

        //private void TxtMsge_OnCompleted(object sender, EventArgs e)
        //{
        //    if (TxtMsge.Text != null)
        //        BtnNext.IsEnabled = TxtMsge.Text.Length > 0;
        //}

        private void TxtMsge_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ProgressBarProgress = 0.0;
            BtnNext.IsEnabled = e.NewTextValue.Length > 0;
            if (e.NewTextValue.Length > 0)
            {
                if (e.NewTextValue.Length <= 30)
                {
                    if (!char.IsLetterOrDigit(e.NewTextValue[e.NewTextValue.Length - 1]) && !char.IsWhiteSpace(e.NewTextValue[e.NewTextValue.Length - 1]))
                    {
                        TxtMsge.Text = e.OldTextValue;
                    }
                }
                else
                {
                    TxtMsge.Text = e.OldTextValue;
                }

            }
        }
    }
}
