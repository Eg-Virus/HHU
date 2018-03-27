using System;
using System.Collections.Generic;
using AirObservationSystem.HHU.Core.Helpers;
using Xamarin.Forms;
using System.Linq;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class TextMessageSummary
    {
        public TextMessageSummary()
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

            if (Static.MessageDirection == MessageDirection.Operation)
            {
                LblMsgDirection.Text += Resx.Resources.MessageDirectionOperation;
                Static.MessageCategory = MessageCategory.Warrnings;
            }
            else
            {
                LblMsgDirection.Text += Resx.Resources.MessageDirectionObservation;
                Static.MessageCategory = MessageCategory.Notifications;
            }

            LblMsgType.Text = Resx.Resources.MessageTypeNotifications;
            LblMsgBody.Text = Static.Message;
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
            var parmList = new List<object>();
            //observerStatus: 0, 
            //bodyText: "",
            //observerName: "", 
            //retryCount: 1, 
            //dtObservationTime:
            if (Static.MessageCategory == MessageCategory.Notifications || Static.MessageCategory == MessageCategory.Warrnings)
                parmList.Add(Helpers.ObserverStatus.Notification);
            else
                parmList.Add(Helpers.ObserverStatus.Alarm);

            parmList.Add(Static.Message);
            parmList.Add(Static.CurrentUser.UserName);
            parmList.Add("0"); //retryCount
            parmList.Add(DateTime.Now);

             await ViewModel.SendAsync(parmList, MessageType.TextObservation);

            var lst =
                Navigation.NavigationStack.Where(
                    p =>
                        p.GetType() != typeof(Login) && p.GetType() != typeof(CommunicationSelection) &&
                        p.GetType() != typeof(ObservationType) && p.GetType() != typeof(TextMessageSummary)).ToList();
            for (var i = 0; i < lst.Count; i++)
                Navigation.RemovePage(lst[i]);

            await Navigation.PopAsync();
        }



    }
}
