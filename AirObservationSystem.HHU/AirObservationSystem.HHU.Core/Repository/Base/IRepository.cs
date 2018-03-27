using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

//using PagedList;

namespace AirObservationSystem.HHU.Core.Repository.Base
{
    public interface IRepository<T> where T : class, new()
    {

        //Task<List<T>> GetAllAsync();
        //Task<List<T>> GetManyAsync(Expression<Func<T, bool>> where);
        //Task<List<T>> GetAsync<TValue>(Expression<Func<T, bool>> where = null, Expression<Func<T, TValue>> orderBy = null);
        //Task<T> GetAsync(Expression<Func<T, bool>> where);
        //Task<T> GetByIdAsync(int id);
        //IEnumerable<T> AsQueryableAsync();
        //Task<int> InsertAsync(T entity);
        //Task<int> UpdateAsync(T entity);
        //Task<int> DeleteAsync(T entity);

        List<T> GetAllAsync();
        List<T> GetManyAsync(Expression<Func<T, bool>> where);
        List<T> GetAsync<TValue>(Expression<Func<T, bool>> where = null, Expression<Func<T, TValue>> orderBy = null);
        T GetAsync(Expression<Func<T, bool>> where);
        T GetByIdAsync(long id);
        IEnumerable<T> AsQueryableAsync();
        int InsertAsync(T entity);
        int UpdateAsync(T entity);
        int DeleteAsync(T entity);

        //T Add(T entity);
        //void Update(T entity);
        //void Delete(T entity);
        //void Delete(Expression<Func<T, bool>> where);
        //T GetById(long id);
        //T GetById(string id);
        //T Get(Expression<Func<T, bool>> where);
        //IEnumerable<T> GetAll();
        //IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        //IPagedList<T> GetPage<TOrder>(Page page, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order);
    }
}