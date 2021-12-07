using Microsoft.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Infrastructure
{
    /// <summary>
    /// Abstract class RepositoryBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T>:IRepository<T> where T : class
    {
        #region Properties
        private SurveyOnlineContext dataContext;

        /// <summary>
        /// DbFactory
        /// </summary>
        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        /// <summary>
        /// Enterty DBContext
        /// </summary>
        protected SurveyOnlineContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbFactory"></param>
        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        #region Implementation

        /// <summary>
        /// Implement Add entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task Add(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            //DbContext.SaveChanges();
        }


        /// <summary>
        /// Implement Add list entity
        /// </summary>
        /// <param name="entities"></param>
        public virtual async Task Add(List<T> entities)
        {
            foreach (T item in entities)
            {
                await DbContext.Set<T>().AddAsync(item);
            }
        }

        /// <summary>
        /// Implement Update entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task Update(T entity)
        {
            DbContext.Set<T>().Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
            //await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Implement update list entity
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Update(List<T> entities)
        {
            foreach (T item in entities)
            {
                DbContext.Set<T>().Attach(item);
                dataContext.Entry(item).State = EntityState.Modified;
            }
        }


        /// <summary>
        /// Implement delete entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }


        /// <summary>
        /// Implement delete list entity
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Delete(List<T> entities)
        {
            foreach (T obj in entities)
                DbContext.Set<T>().Remove(obj);
        }

        /// <summary>
        /// Implement get entity by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> GetById(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Implement get all entity
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Implement entities
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where)
        {
            return await DbContext.Set<T>().Where(where).ToListAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> where)
        {
            return await DbContext.Set<T>().Where(where).FirstOrDefaultAsync<T>();
        }

        public virtual async Task<int> Commit()
        {
           return await DbContext.SaveChangesAsync();
        }
        #endregion
    }
}
