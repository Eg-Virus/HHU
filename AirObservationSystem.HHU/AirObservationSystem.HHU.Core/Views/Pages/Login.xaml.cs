
using System;
using System.Collections.Generic;
using AirObservationSystem.HHU.Core.Helpers;
using AirObservationSystem.HHU.Core.Model.Old_Code;
using Plugin.Toasts;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class Login
    {
        public Login()
        {
            InitializeComponent();

            AfterLocalInitialize();



        }

        private async void ViewModel_DataReceivedLogin(object sender, object e)
        {
            ViewModel.DataReceivedLogin -= ViewModel_DataReceivedLogin;

            //check for the validation of the login
            LoginResponseBody loginResponse = (LoginResponseBody)e;
            if (loginResponse.UserId != -1) //Good to go
            {
                Static.CurrentUser.UserId = loginResponse.UserId;
                Static.CurrentUser.IsLogedIn = true;
                //Check if there's any changes in the FIU before login.
                await ViewModel.SendAsync(ViewModel.StatusRequest(StatusRequestType.LoginRequest, RequestPacketTypes.Initial, 0),
                                          MessageType.MessagesStatusRequest);

                await Navigation.PushAsync(new ObservationType(), true);
            }
            else //Failed
            {
                Static.CurrentUser.UserId = -1;
                Static.CurrentUser.IsLogedIn = false;
                if (Static.CurrentUser.LogInMsgId > 0) Static.CurrentUser.LogInMsgId -= 1;

                await DisplayAlert(Resx.Resources.BtnLogin, Resx.Resources.LoginFailed, Resx.Resources.Yes,
                    Resx.Resources.No);
            }
        }

        public override void OnLanguageChaged(string language)
        {
            base.OnLanguageChaged(language);
            if (language == "ar")
            {
                TxtPassword.HorizontalTextAlignment = TextAlignment.End;
                TxtUsername.HorizontalTextAlignment = TextAlignment.End;
            }
            else
            {
                TxtPassword.HorizontalTextAlignment = TextAlignment.Start;
                TxtUsername.HorizontalTextAlignment = TextAlignment.Start;
            }

        }

        public override void InitializeLocalProperites()
        {
            base.InitializeLocalProperites();

            LblAplicationName.Text = Resx.Resources.LblAplicationName;
            TxtPassword.Placeholder = Resx.Resources.TxtPassword;
            TxtPassword.Text = string.Empty;
            TxtUsername.Placeholder = Resx.Resources.TxtUsername;
            TxtUsername.Text = string.Empty;
            BtnLogin.Text = Resx.Resources.BtnLogin;
        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();

            PageInfoGridVisible = false;
            ProgressBarGridVisible = false;
            StopProgress = true;
        }

        private async void BtnLogin_OnClicked(object sender, EventArgs e)
        {
            //Send the Login msg request
            if (TxtUsername.Text.Length > 0 && TxtPassword.Text.Length > 0)
            {
                ViewModel.DataReceivedLogin += ViewModel_DataReceivedLogin;
                if (Static.CurrentUser.LogInMsgId >= 0)
                {
                    Static.CurrentUser.UserName = TxtUsername.Text;
                    Static.CurrentUser.Password = TxtPassword.Text;

                    await ViewModel.SendAsync(Static.CurrentUser,
                        MessageType.LoginRequest);
                }
                else
                {
                    await Navigation.PopToRootAsync(true);
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            //new thread
            Device.BeginInvokeOnMainThread(async () =>
            {

                if (await DisplayAlert(Resx.Resources.ChangeConnectionTitle, Resx.Resources.ChangeConnectionMessage, Resx.Resources.Yes, Resx.Resources.No))
                {
                    //other methods
                    await Navigation.PopAsync();
                }

            });

            return true; //Do not navigate backwards by pressing the button
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AfterLocalInitialize();
#if DEBUG
            TxtUsername.Text = "n";
            TxtPassword.Text = "123";
#endif
        }
    }
}
