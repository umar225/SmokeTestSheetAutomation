using Coursewise.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Generics
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
    {
        private ICoursewiseDbContext dbContext;
        private DbSet<TEntity> dbSet;

        public DbSet<TEntity> DbSet
        {
            get { return dbSet; }
            private set { dbSet = value; }
        }

        public ICoursewiseDbContext DbContext
        {
            get { return dbContext; }
            private set { dbContext = value; }
        }

        public GenericRepository(ICoursewiseDbContext dbContext)
        {
            this.dbContext = dbContext;

            dbSet = dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetAsync(TKey id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await dbSet.FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public virtual async Task<TEntity> GetAsync(params object[] keys)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await dbSet.FindAsync(keys);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where)
        {
            return await dbSet.Where(where).ToListAsync();
        }


        public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public virtual IQueryable<TEntity> Get()
        {
            return dbSet.AsQueryable();
        }

        private TEntity Add(TEntity entity)
        {
            var entry = dbSet.Add(entity).Entity;

            return entry;
        }


        public virtual void Add(List<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual Task<TEntity> AddAsync(TEntity entity)
        {
            return Task.FromResult(Add(entity));
        }
        public virtual async Task AddAsync(List<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }



        private void Update(TEntity entityToUpdate)
        {
            dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
        private void Update(List<TEntity> entitiesToUpdate)
        {
            foreach (var entity in entitiesToUpdate)
            {
                Update(entity);
            }
        }
        public virtual Task UpdateAsync(TEntity entity)
        {
            return Task.Run(() => Update(entity));
        }

        public virtual Task UpdateAsync(List<TEntity> entities)
        {
            return Task.Run(() => Update(entities));
        }



        private void Delete(TEntity entity)
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }
        public virtual async Task DeleteAsync(TKey id)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            TEntity entityToDelete = await dbSet.FindAsync(id);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
            }
        }
        public virtual Task DeleteAsync(TEntity entity)
        {
            return Task.Run(() => Delete(entity));
        }

        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            dbSet.RemoveRange(await dbSet.Where(predicate).ToListAsync());
        }

        public virtual async Task ReloadEntity(TEntity entityToReload)
        {
            await dbContext.Entry(entityToReload).ReloadAsync();
        }

        public virtual async Task ReloadEntity(List<TEntity> entitiesToReload)
        {
            foreach (var entity in entitiesToReload)
            {
                await ReloadEntity(entity);
            }
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            TEntity entity;
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (filter != null)
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                entity = await query.FirstOrDefaultAsync(filter);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }
            else
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                entity = await query.FirstOrDefaultAsync();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }
#pragma warning disable CS8603 // Possible null reference return.
            return entity;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
