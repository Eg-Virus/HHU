using System;
using System.Collections.Generic;
using System.Linq;
using AirObservationSystem.HHU.Core.Helpers;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.Message;
using AirObservationSystem.HHU.Core.Repository.Message;
using Autofac;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class Reports
    {
        public Reports()
        {
            InitializeComponent();

            AfterLocalInitialize();

        }

        public override void InitializeLocalProperites()
        {
            base.InitializeLocalProperites();

            LblPageName.Text = Resx.Resources.LblReportsPageName;
            LblFilter.Text = Resx.Resources.LblFilter;
            BtnBack.Image = Static.GetImageSource("back.png") as FileImageSource;
            BtnBack.Text = Resx.Resources.BtnBack;
            BtnRefresh.Image = Static.GetImageSource("refresh.png") as FileImageSource;

            //TODO:Apply Localization here
            PkrFilter.Items.Add("Sent Messages");     //Item : 0
            PkrFilter.Items.Add("Recieved Messages"); //Item :1
            PkrFilter.SelectedIndex = -1;


        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();


        }

        public override void OnLanguageChaged(string language)
        {
            base.OnLanguageChaged(language);

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

        private void PkrFilter_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadListView();
        }

        private void LoadListView()
        {
            BtnRefresh.IsEnabled = false;
            if (PkrFilter.SelectedIndex == 0) //Sent Messages
            {
                LvSent.IsVisible = true;
                LvRecieved.IsVisible = false;
                GrdSentHeader.IsVisible = true;
                GrdRecieveHeader.IsVisible = false;

                LvSent.ItemsSource =  AppContainer.Container.Resolve<SentMessageRepository>().GetAllAsync();
            }
            else if (PkrFilter.SelectedIndex == 1) //Recieved Messages
            {
                LvRecieved.IsVisible = true;
                LvSent.IsVisible = false;
                GrdRecieveHeader.IsVisible = true;
                GrdSentHeader.IsVisible = false;

                LvRecieved.ItemsSource = AppContainer.Container.Resolve<RecievedMessageRepository>().GetAllAsync();
            }
            else // -1
            {
                //LvRecieved.IsVisible = false;
                //LvSent.IsVisible = false;
                //GrdSentHeader.IsVisible = false;
                //GrdRecieveHeader.IsVisible = false;
            }
            BtnRefresh.IsEnabled = true;
        }

        private void BtnBack_OnClicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        private void LvSent_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var msg = (SentMessage)e.SelectedItem;
                msg.Status = AppContainer.Container.Resolve<SentMessageRepository>().GetByIdAsync(msg.Id).Status;
                ((List<SentMessage>)LvSent.ItemsSource).Find(m => m.Id == msg.Id).Status = msg.Status;
            }
        }

        private void LvRecieved_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
        }

        private void BtnRefresh_OnClicked(object sender, EventArgs e)
        {
            LoadListView();
        }
    }
}
