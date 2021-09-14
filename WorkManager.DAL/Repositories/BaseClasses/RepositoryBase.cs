using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities.Interfaces;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories.BaseClasses
{
	public abstract class RepositoryBase<TEntity> :IRepository<TEntity> where TEntity : class, IEntity, new()
	{
		protected DbContext.Interfaces.IDbContextFactory<WorkManagerDbContext> DbContextFactory;

		protected RepositoryBase(DbContext.Interfaces.IDbContextFactory<WorkManagerDbContext> dbContextFactory)
		{
			DbContextFactory = dbContextFactory;
		}

		public IEnumerable<TEntity> GetAll()
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				return GetAllInt(dbContext.GetDatabaseByType<TEntity>().AsNoTracking());
			}
		}

		protected abstract IEnumerable<TEntity> GetAllInt(IQueryable<TEntity> dbSet);

		public async Task<List<TEntity>> GetAllAsync(CancellationToken token = default)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				return await dbContext.GetDatabaseByType<TEntity>().ToListAsync(cancellationToken:token);
			}
		}

		public TEntity GetById(Guid id)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				return dbContext.GetDatabaseByType<TEntity>().FirstOrDefault(s => s.Id == id);
			};
		}

		public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken token = default)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				return await dbContext.GetDatabaseByType<TEntity>().FirstOrDefaultAsync(s => s.Id == id, cancellationToken: token);
			}
		}

		public bool Remove(Guid id)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
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

		public async Task<bool> RemoveAsync(Guid id, CancellationToken token = default)
		{
			return await Task.Run(() => Remove(id), token);
		}

		public virtual TEntity Add(TEntity entity)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
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

		public async Task<TEntity> AddAsync(TEntity entity, CancellationToken token = default)
		{
			return await Task.Run(() => Add(entity), token);
		}

		public void Update(TEntity entity)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				if (entity == null)
					throw new ArgumentNullException();
				if (dbContext.GetDatabaseByType<TEntity>().All(s => s.Id != entity.Id))
					dbContext.GetDatabaseByType<TEntity>().Add(entity);
				TEntity entry = dbContext.GetDatabaseByType<TEntity>().FirstOrDefault(s => s.Id == entity.Id);
				if (entry != null)
				{
					dbContext.Entry<TEntity>(entry).CurrentValues.SetValues(entity);
					dbContext.SaveChanges();
				}
			}
		}

		public async Task UpdateAsync(TEntity entity, CancellationToken token = default)
		{
			await Task.Run(() => Update(entity), token);
		}

		public bool Exists(TEntity entity)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				return dbContext.GetDatabaseByType<TEntity>().Any(s => s.Equals(entity));
			}
		}

		public async Task<bool> ExistsAsync(TEntity entity, CancellationToken token = default)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				return await dbContext.GetDatabaseByType<TEntity>().AnyAsync(s => s.Equals(entity) || (entity != null && s.Id == entity.Id), token);
			}
		}

		public void Clear()
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				DbSet<TEntity> dbSet = dbContext.GetDatabaseByType<TEntity>();
				foreach (var id in dbSet.Select(e => e.Id))
				{
					var entity = new TEntity() { Id = id };
					dbSet.Attach(entity);
					dbSet.Remove(entity);
				}
				dbContext.SaveChanges();
			}
		}

		public async Task ClearAsync(CancellationToken token = default)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				DbSet<TEntity> dbSet = dbContext.GetDatabaseByType<TEntity>();
				foreach (var id in dbSet.Select(e => e.Id))
				{
					var entity = new TEntity() { Id = id };
					dbSet.Attach(entity);
					dbSet.Remove(entity);
				}
				await dbContext.SaveChangesAsync(token);
			}
		}
	}
}