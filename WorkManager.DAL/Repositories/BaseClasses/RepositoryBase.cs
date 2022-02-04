

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities.Interfaces;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories.BaseClasses
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        protected WorkManagerDbContext DbContext { get; }

        protected RepositoryBase(WorkManagerDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbContext.Database.EnsureCreated();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token)
        {
            return await DbContext.Set<TEntity>().AsQueryable().ToListAsync(token);
        }

        public IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().Where(predicate).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token)
        {
            return await DbContext.Set<TEntity>().Where(predicate).ToListAsync(token);
        }

        public IEnumerable<TEntity> GetWhereOrderBy<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy)
        {
            return DbContext.Set<TEntity>().Where(predicate).OrderBy(orderBy).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetWhereOrderByAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, CancellationToken token)
        {
            return await DbContext.Set<TEntity>().Where(predicate).OrderBy(orderBy).ToListAsync(token);
        }

        public IEnumerable<TEntity> GetWhereOrderByDescending<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy)
        {
            return DbContext.Set<TEntity>().Where(predicate).OrderByDescending(orderBy).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetWhereOrderByDescendingAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy, CancellationToken token)
        {
            return await DbContext.Set<TEntity>().Where(predicate).OrderByDescending(orderBy).ToListAsync(token);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token)
        {
            return await DbContext.Set<TEntity>().CountAsync(predicate, token);
        }

        public TEntity GetById(Guid id)
        {
            return DbContext.Set<TEntity>().FirstOrDefault(s => s.Id == id);
        }

        public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await DbContext.Set<TEntity>().AsQueryable().FirstOrDefaultAsync(s => s.Id == id, token);
        }

        public bool Remove(Guid id)
        {
            TEntity entity = GetById(id);
            if (entity == null)
                return false;
            if (DbContext.Set<TEntity>().Remove(entity) != null)
            {
                DbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveAsync(Guid id, CancellationToken token)
        {
            TEntity entity = await GetByIdAsync(id, token);
            if (entity == null)
                return false;
            if (DbContext.Set<TEntity>().Remove(entity) != null)
            {
                await DbContext.SaveChangesAsync(token);
                return true;
            }
            return false;
        }

        public virtual TEntity Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException();
            if (!Exists(entity))
                if (DbContext.Set<TEntity>().Add(entity) != null)
                {
                    AddInt(entity, DbContext);
                    DbContext.SaveChanges();
                    return entity;
                }
            return default;
        }

        protected abstract void AddInt(TEntity entity, WorkManagerDbContext dbContext);

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken token)
        {
            if (entity == null)
                throw new ArgumentNullException();
            if (await ExistsAsync(entity, token))
                return default;
            if (await DbContext.Set<TEntity>().AddAsync(entity, token) != null)
            {
                AddInt(entity, DbContext);
                await DbContext.SaveChangesAsync(token);
                return entity;
            }
            return default;
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException();
            if (DbContext.Set<TEntity>().All(s => s.Id != entity.Id))
                DbContext.Set<TEntity>().Add(entity);
            TEntity entry = DbContext.Set<TEntity>().FirstOrDefault(s => s.Id == entity.Id);
            if (entry != null)
            {
                DbContext.Entry(entry).CurrentValues.SetValues(entity);
                DbContext.SaveChanges();
            }
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken token)
        {
            if (entity == null)
                throw new ArgumentNullException();
            if (await DbContext.Set<TEntity>().AsQueryable().AllAsync(s => s.Id != entity.Id, token))
                DbContext.Set<TEntity>().Add(entity);
            TEntity entry = await DbContext.Set<TEntity>().AsQueryable().FirstOrDefaultAsync(s => s.Id == entity.Id, token);
            if (entry != null)
            {
                DbContext.Entry(entry).CurrentValues.SetValues(entity);
                await DbContext.SaveChangesAsync(token);
            }
        }

        public bool Exists(TEntity entity)
        {
            return DbContext.Set<TEntity>().Any(s => s.Equals(entity));
        }

        public async Task<bool> ExistsAsync(TEntity entity, CancellationToken token)
        {
            return await DbContext.Set<TEntity>().AsQueryable().AnyAsync(s => s.Equals(entity) || (entity != null && s.Id == entity.Id), token);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().AsQueryable().Any(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token)
        {
            return await DbContext.Set<TEntity>().AsQueryable().AnyAsync(predicate, token);
        }

        public void Clear()
        {
            DbSet<TEntity> dbSet = DbContext.Set<TEntity>();
            foreach (var id in dbSet.AsQueryable().Select(e => e.Id))
            {
                var entity = new TEntity { Id = id };
                dbSet.Attach(entity);
                dbSet.Remove(entity);
            }
            DbContext.SaveChanges();
        }

        public async Task ClearAsync(CancellationToken token)
        {
            DbSet<TEntity> dbSet = DbContext.Set<TEntity>();
            foreach (var id in dbSet.AsQueryable().Select(e => e.Id))
            {
                var entity = new TEntity { Id = id };
                dbSet.Attach(entity);
                dbSet.Remove(entity);
            }
            await DbContext.SaveChangesAsync(token);
        }
    }
}