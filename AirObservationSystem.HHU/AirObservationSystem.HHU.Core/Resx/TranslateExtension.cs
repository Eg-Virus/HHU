using System;
using System.Globalization;
using AirObservationSystem.HHU.Core.PlatformInterface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AirObservationSystem.HHU.Core.Infrastructure;
using Autofac;

namespace AirObservationSystem.HHU.Core.Resx
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private readonly CultureInfo _cultureInfo;

        public TranslateExtension()
        {
            //if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
            {
                _cultureInfo = AppContainer.Container.Resolve<ICultureInfo>().GetCurrentCultureInfo();
            }
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return null;
            }

            var translation = Resources.ResourceManager.GetString(Text, _cultureInfo);

#if DEBUG
            if (translation == null)
            {
                throw new ArgumentException($"Key {Text} was not found for culture {_cultureInfo}.");
            }
#endif
            return translation;
        }
    }
}
