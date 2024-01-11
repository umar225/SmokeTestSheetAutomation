using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Generics
{
    public interface IGenericRepository<TEntity, TKey>
    {
        /// <summary>
        /// Asynchronously get by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TKey id);

        /// <summary>
        /// Asynchronously get by composite keys
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(params object[] keys);

        /// <summary>
        /// Asynchronously get more than one with expression
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// To get entity with childs and sort filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Get All
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Get();

        /// <summary>
        /// To add entity
        /// </summary>
        /// <param name="entity"></param>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// To add list of items
        /// </summary>
        /// <param name="entity"></param>
        Task AddAsync(List<TEntity> entities);

        /// <summary>
        /// To update entity
        /// </summary>
        /// <param name="entity">
        /// </param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Update the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        Task UpdateAsync(List<TEntity> entities);

        /// <summary>
        ///Asynchronously delete by Id
        /// </summary>
        /// <param>
        ///     <name>id</name>
        /// </param>
        Task DeleteAsync(TKey id);

        /// <summary>
        /// Delete entity
        /// </summary>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// To reload entity
        /// </summary>
        /// <param name="entityToReload">
        /// </param>
        Task ReloadEntity(TEntity entityToReload);

        /// <summary>
        /// To reload entities
        /// </summary>
        /// <param name="entitiesToReload">
        /// </param>
        Task ReloadEntity(List<TEntity> entitiesToReload);

        /// <summary>
        /// To get the first entity
        /// </summary>
        /// <param name="filter"></param>
        /// /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "");
    }
}
