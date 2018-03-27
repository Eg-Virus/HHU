using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AirObservationSystem.HHU.Core.Infrastructure;
using SQLite.Net.Async;

namespace AirObservationSystem.HHU.Core.Repository.Base
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class, new()
    {
        public RepositoryBase(IDatabaseFactory dbFactory)
        {
            DatabaseFactory = dbFactory;

            DbAsyncConnection = DatabaseFactory.DbConnection;

            CreateDbIfNotExist();
        }

        protected IDatabaseFactory DatabaseFactory
        { get; private set; }

        public SQLiteAsyncConnection DbAsyncConnection { get; private set; }

        private void CreateDbIfNotExist() => DatabaseFactory.CreateDbIfNotExist<T>();

        public IEnumerable<T> AsQueryableAsync() =>
             DbAsyncConnection.Table<T>().ToListAsync().GetAwaiter().GetResult();

        public List<T> GetAllAsync() =>
             DbAsyncConnection.Table<T>().ToListAsync().GetAwaiter().GetResult();

        public List<T> GetAsync<TValue>(Expression<Func<T, bool>> where = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = DbAsyncConnection.Table<T>();

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                query = query.OrderBy(orderBy);

            return query.ToListAsync().GetAwaiter().GetResult();
        }

        public T GetByIdAsync(long id) =>
             DbAsyncConnection.FindAsync<T>(id).GetAwaiter().GetResult();
        /// <summary>
        /// Return One Object
        /// </summary>
        public T GetAsync(Expression<Func<T, bool>> where) =>
            DbAsyncConnection.FindAsync(where).GetAwaiter().GetResult();
        /// <summary>
        /// Return List
        /// </summary>
        public List<T> GetManyAsync(Expression<Func<T, bool>> where)
        {
            var query = DbAsyncConnection.Table<T>();

            if (where != null)
                query = query.Where(where);

            return query.ToListAsync().GetAwaiter().GetResult();
        }
        public int InsertAsync(T entity) =>
             DbAsyncConnection.InsertAsync(entity).GetAwaiter().GetResult();

        public int UpdateAsync(T entity) =>
              DbAsyncConnection.UpdateAsync(entity).GetAwaiter().GetResult();

        public int DeleteAsync(T entity) =>
             DbAsyncConnection.DeleteAsync(entity).GetAwaiter().GetResult();
        

    }
}
