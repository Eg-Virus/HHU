using System.Reflection;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.Message;
using AirObservationSystem.HHU.Core.Repository.Base;
using AirObservationSystem.HHU.Core.Repository.Message;
using AirObservationSystem.HHU.Core.ViewModels;
using Autofac;

namespace AirObservationSystem.HHU.Core.Infrastructure
{
    public class AppSetup
    {
        public IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            return containerBuilder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder cb)
        {
            cb.RegisterType<DatabaseFactory>().As(typeof(IDatabaseFactory));

            // Register Repositories 
            //cb.RegisterAssemblyTypes(typeof(RecievedMessageRepository).GetTypeInfo().Assembly)
            //    .Where(t => t.Name.EndsWith("Repository"))
            //    .AsImplementedInterfaces().InstancePerRequest();
            cb.RegisterType<RecievedMessageRepository>();
            cb.RegisterType<SentMessageRepository>();
            // Register Services 
            //cb.RegisterAssemblyTypes(typeof(UserService).Assembly)
            //    .Where(t => t.Name.EndsWith("Service"))
            //    .AsImplementedInterfaces().InstancePerRequest();

            // Register View Models
            cb.RegisterType<MainViewModel>().SingleInstance();
        }
    }
}