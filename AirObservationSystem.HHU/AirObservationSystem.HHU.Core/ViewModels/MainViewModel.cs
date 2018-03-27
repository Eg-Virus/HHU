using AirObservationSystem.HHU.Core.PlatformInterface;

namespace AirObservationSystem.HHU.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel(IPlatform platform, ICultureInfo localService, IMedia media, ICrc32 crc32) : base(platform, localService, media, crc32)
        {

        }
    }
}
