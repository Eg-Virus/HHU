using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.Droid.Implementations;
using Autofac;

namespace AirObservationSystem.HHU.Droid
{
    public class Setup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder cb)
        {
            base.RegisterDependencies(cb);

            cb.RegisterType<PlatformCultureInfo>().As<ICultureInfo>();
            cb.RegisterType<DroidPlatform>().As<IPlatform>();
            //cb.RegisterType<SerialPort>().As<IMedia>();
            cb.RegisterType<Crc32>().As<ICrc32>();
        }
    }
    
}