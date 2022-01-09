

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities.Interfaces;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories.BaseClasses
{
	public abstract class RepositoryBase<TEntity> :IRepository<TEntity> where TEntity : class, IEntity, new()
	{
		protected DbContext.Interfaces.IDBContextFactory<WorkManagerDbContext> IdbContextFactory;

		protected RepositoryBase(DbContext.Interfaces.IDBContextFactory<WorkManagerDbContext> idbContextFactory)
		{
			IdbContextFactory = idbContextFactory;
		}

		public ICollection<TEntity> GetAll()
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return GetAllInt(dbContext.GetDatabaseByType<TEntity>().AsNoTracking()).ToList();
			}
		}

		protected abstract ICollection<TEntity> GetAllInt(IQueryable<TEntity> dbSet);

		public async Task<ICollection<TEntity>> GetAllAsync(CancellationToken token)
		{
			using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
			{
				return await AsyncEnumerable.ToListAsync(dbContext.GetDatabaseByType<TEntity>(), token);
			}
		}

		public TEntity GetById(Guid id)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return dbContext.GetDatabaseByType<TEntity>().FirstOrDefault(s => s.Id == id);
			};
		}

		public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken token)
		{
			using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
			{
				return await AsyncEnumerable.FirstOrDefaultAsync(dbContext.GetDatabaseByType<TEntity>(), s => s.Id == id, token);
			}
		}

		public bool Remove(Guid id)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				TEntity entity = GetById(id);
				if (entity == null)
					return false;
				if (dbContext.GetDatabaseByType<TEntity>().Remove(entity) != null)
				{
					dbContext.SaveChanges();
					return true;
				}
			}
			return false;
		}

		public async Task<bool> RemoveAsync(Guid id, CancellationToken token)
		{
            using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
            {
                TEntity entity = await GetByIdAsync(id, token);
                if (entity == null)
                    return false;
                if (dbContext.GetDatabaseByType<TEntity>().Remove(entity) != null)
                {
                    await dbContext.SaveChangesAsync(token);
                    return true;
                }
            }
            return false;
		}

		public virtual TEntity Add(TEntity entity)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				if (entity == null)
					throw new ArgumentNullException();
				if (!Exists(entity))
					if (dbContext.GetDatabaseByType<TEntity>().Add(entity) != null)
					{
						AddInt(entity, dbContext);
						dbContext.SaveChanges();
						return entity;
					}
			}
			return default;
		}

		protected abstract void AddInt(TEntity entity, WorkManagerDbContext dbContext);

		public async Task<TEntity> AddAsync(TEntity entity, CancellationToken token)
		{
            using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
            {
                if (entity == null)
                    throw new ArgumentNullException();
                if (!await ExistsAsync(entity, token))
                    if (await dbContext.GetDatabaseByType<TEntity>().AddAsync(entity,token) != null)
                    {
                        AddInt(entity, dbContext);
                        await dbContext.SaveChangesAsync(token);
                        return entity;
                    }
            }
            return default;
		}

		public void Update(TEntity entity)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				if (entity == null)
					throw new ArgumentNullException();
				if (dbContext.GetDatabaseByType<TEntity>().All(s => s.Id != entity.Id))
					dbContext.GetDatabaseByType<TEntity>().Add(entity);
				TEntity entry = dbContext.GetDatabaseByType<TEntity>().FirstOrDefault(s => s.Id == entity.Id);
				if (entry != null)
				{
					dbContext.Entry(entry).CurrentValues.SetValues(entity);
					dbContext.SaveChanges();
				}
			}
		}

		public async Task UpdateAsync(TEntity entity, CancellationToken token)
		{
            using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
            {
                if (entity == null)
                    throw new ArgumentNullException();
                if (await AsyncEnumerable.AllAsync(dbContext.GetDatabaseByType<TEntity>(), s => s.Id != entity.Id, token))
                    dbContext.GetDatabaseByType<TEntity>().Add(entity);
                TEntity entry = await AsyncEnumerable.FirstOrDefaultAsync(dbContext.GetDatabaseByType<TEntity>(), s => s.Id == entity.Id, token);
                if (entry != null)
                {
                    dbContext.Entry(entry).CurrentValues.SetValues(entity);
                    await dbContext.SaveChangesAsync(token);
                }
            }
		}

		public bool Exists(TEntity entity)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return dbContext.GetDatabaseByType<TEntity>().Any(s => s.Equals(entity));
			}
		}

		public async Task<bool> ExistsAsync(TEntity entity, CancellationToken token)
		{
			using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
			{
				return await AsyncEnumerable.AnyAsync(dbContext.GetDatabaseByType<TEntity>(), s => s.Equals(entity) || (entity != null && s.Id == entity.Id), token);
			}
		}

		public void Clear()
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				DbSet<TEntity> dbSet = dbContext.GetDatabaseByType<TEntity>();
				foreach (var id in dbSet.AsQueryable().Select(e => e.Id))
				{
					var entity = new TEntity { Id = id };
					dbSet.Attach(entity);
					dbSet.Remove(entity);
				}
				dbContext.SaveChanges();
			}
		}

		public async Task ClearAsync(CancellationToken token)
		{
			using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
			{
				DbSet<TEntity> dbSet = dbContext.GetDatabaseByType<TEntity>();
				foreach (var id in dbSet.AsQueryable().Select(e => e.Id))
				{
					var entity = new TEntity { Id = id };
					dbSet.Attach(entity);
					dbSet.Remove(entity);
				}
				await dbContext.SaveChangesAsync(token);
			}
		}
	}
}