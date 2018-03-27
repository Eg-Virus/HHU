

using System;
using System.Collections.Generic;
using System.Linq;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Views.Pages
{
    public partial class Lock
    {
        public Lock()
        {
            InitializeComponent();

            AfterLocalInitialize();

            //Send the Lock Message
            ViewModel.SendAsync(null, MessageType.Lock);
        }

        public override void InitializeLocalProperites()
        {
            base.InitializeLocalProperites();

            StopProgress = true;

            LblBodyMsg.Text = Resx.Resources.LockMsg;
            BtnUnlock.Text = Resx.Resources.BtnUnlock;
            Img.Source = Static.GetImageSource("lock_big.png");

            Static.IsLocked = true;
        }

        public override void InitializeBindableProperties()
        {
            base.InitializeBindableProperties();

            ProgressBarGridVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StopProgress = true;
        }

        private async void BtnUnlock_OnClicked(object sender, EventArgs e)
        {
            //var lst =
            //    Navigation.NavigationStack.Where(
            //        p =>
            //            p.GetType() != typeof(Login) && p.GetType() != typeof(CommunicationSelection) &&
            //            p.GetType() != typeof(Lock)).ToList();
            //for (var i=0;i<lst.Count ; i++)
            //    Navigation.RemovePage(lst[i]);

            //await Navigation.PopAsync();

            Static.IsLocked = false;

            await Static.PopPagesAsync(new List<Type> { typeof(Login), typeof(CommunicationSelection), typeof(Lock) });
        }
    }
}
