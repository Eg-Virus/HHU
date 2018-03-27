using System;
using System.Threading.Tasks;
using AirObservationSystem.HHU.Core.Helpers;
using AirObservationSystem.HHU.Core.Model.Message;
using SQLite.Net.Async;

namespace AirObservationSystem.HHU.Core.Infrastructure
{
    public interface IDatabaseFactory 
    {
        SQLiteAsyncConnection DbConnection { get;  }

        void CreateDbIfNotExist<T>() where T : class;
    }
}
