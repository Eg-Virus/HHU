using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Views.Pages;
using Plugin.Toasts.UWP;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            

            var setup = new Setup();
            AppContainer.Container = setup.CreateContainer();

            

            LoadApplication(new Core.App(typeof(CommunicationSelection)));
        }
    }
}
