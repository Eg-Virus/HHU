using System.Diagnostics;
using AirObservationSystem.HHU.Core.Base;
using AirObservationSystem.HHU.Core.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace AirObservationSystem.HHU.Core.Views
{
    public partial class MainPage : MainView<MainViewModel>
    {
        public MainPage()
        {
            InitializeComponent();

            //MainViewModel viewModel = null;

            //using (var scope = AppContainer.Container.BeginLifetimeScope())
            //{
            //    viewModel = AppContainer.Container.Resolve<MainViewModel>();
            //}

            var model = ViewModel.GetModel;
            var version = ViewModel.GetVersion;

            Debug.WriteLine(@"Model:{0} Version:{1}", model, version);

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = {
                    new Label {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Text = $@"Model:{ViewModel.GetModel} Version:{ViewModel.GetVersion}"
                    }
                }
            };

            //this.Title = $@"Model:{ViewModel.GetModel} Version:{ViewModel.GetVersion}";
        }
    }
}
