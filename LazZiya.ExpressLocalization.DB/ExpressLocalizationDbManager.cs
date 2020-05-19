using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LazZiya.ExpressLocalization.DB
{
    /// <summary>
    /// Manage CRUD operations for ExpressLocalization entities
    /// </summary>
    public class ExpressLocalizationDbManager<TContext, TExpressLocalizationResource, TCulturesResource, TKey>
        where TContext : DbContext
        where TExpressLocalizationResource : class, IExpressLocalizationEntity<TKey>
        where TCulturesResource : class, IExpressLocalizationCulture<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly TContext Context;

        /// <summary>
        /// Initialize a new instance of ExpressLocalizationDbManager
        /// </summary>
        /// <param name="context"></param>
        public ExpressLocalizationDbManager(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Context = context;
        }

        /// <summary>
        /// Get entity from db
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="searchExpression">search expression</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> searchExpression)
            where T : class
        {
            return await Context.Set<T>().AsNoTracking().SingleOrDefaultAsync(searchExpression);
        }

        /// <summary>
        /// Add a new entity to the db
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync<T>(T entity)
            where T : class
        {
            Context.Attach(entity).State = EntityState.Added;

            return await Context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T>(T entity)
        {
            Context.Attach(entity).State = EntityState.Modified;

            return await Context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<T>(T entity)
        {
            Context.Attach(entity).State = EntityState.Deleted;

            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<(ICollection<T>, int)> ListAsync<T>(int start, int count, List<Expression<Func<T, bool>>> searchExpressions, List<Expression<Func<T, object>>> orderBy)
        {

        }
    }
}
