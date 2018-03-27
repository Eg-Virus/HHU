using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using AirObservationSystem.HHU.Core.Helpers;
using AirObservationSystem.HHU.Core.Model.Message;
using AirObservationSystem.HHU.Core.PlatformInterface;
using Autofac;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using Result = AirObservationSystem.HHU.Core.Helpers.Result;

namespace AirObservationSystem.HHU.Core.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private SQLiteAsyncConnection _dbConnection;
        private readonly ISQLitePlatform _iSqLite;
        private readonly string _path;

        public DatabaseFactory(ISQLitePlatform iSqLite)
        {
            _iSqLite = iSqLite;

            _path = AppContainer.Container.Resolve<IPlatform>().GetDbPath(Static.DbName);

            DbConnection = Static.GetConnection(_path, _iSqLite);
        }

        public SQLiteAsyncConnection DbConnection
        {
            get
            {
                //if (_dbConnection == null)
                //{
                //    LazyInitializer.EnsureInitialized(ref _dbConnection, DependencyService.Get<ISQLite>().GetAsyncConnection);
                //}
                //GetAsyncConnection();
                return _dbConnection;
            }
            private set { _dbConnection = value; }
        }

        public void CreateDbIfNotExist<T>() where T : class
        {
            try
            {
                var r = DbConnection.CreateTableAsync<T>().GetAwaiter().GetResult();

                Debug.WriteLine("Create Table success!");
            }
            catch (Exception ex)
            {
                Static.Log("CreateDbIfNotExist", LogType.Error, ex.Message);

            }

        }

        protected override void DisposeCore()
        {
            //if (_dbConnection != null)
            //_dbConnection.Dispose();
            _dbConnection = null;
        }

        //public SQLiteAsyncConnection GetAsyncConnection()
        //{
        //    if (_dbConnection == null)
        //    {
        //        var connectionFactory =
        //            new Func<SQLiteConnectionWithLock>(() =>
        //                new SQLiteConnectionWithLock(_iSqLite, new SQLiteConnectionString(_path, false)));
        //        _dbConnection = new SQLiteAsyncConnection(connectionFactory);
        //    }
        //    return _dbConnection;
        //}


    }

}
