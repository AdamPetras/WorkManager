using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities.Interfaces;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades.BaseClasses
{
	public abstract class FacadeBase<TModel, TEntity> : IFacade<TModel> where TModel : IModel where TEntity : class, IEntity
    {
        protected readonly IMapper<TEntity, TModel> Mapper;

		protected FacadeBase(WorkManagerDbContext dbContext, IMapper<TEntity, TModel> mapper, IDatabaseSessionController databaseSessionController)
		{
            DatabaseSessionController = databaseSessionController;
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbContext.Database.EnsureCreated();
			Mapper = mapper;
		}

        public WorkManagerDbContext DbContext { get; }
        public IDatabaseSessionController DatabaseSessionController { get; }

		public virtual TModel Add(TModel model)
		{
            if (model == null)
                throw new ArgumentNullException();
            DatabaseSessionController.Reset();
            using (IDbContextTransaction tx = DbContext.Database.BeginTransaction())
            {
                if (DbContext.Add(Mapper.Map(model)) == null)
                    return default;
                tx.Commit();
                return model;
            }
        }

		public virtual async Task<TModel> AddAsync(TModel model, CancellationToken token = default)
		{

			if (model == null)
                throw new ArgumentNullException();
            DatabaseSessionController.Reset();
            using (IDbContextTransaction tx = await DbContext.Database.BeginTransactionAsync(token))
            {
                if (await DbContext.AddAsync(Mapper.Map(model), token).ConfigureAwait(false) == null)
                {
                    return default;
                }
                await tx.CommitAsync(token);
                return model;
            }
        }

        public virtual bool Remove(Guid id)
		{
            DatabaseSessionController.Reset();
            using (IDbContextTransaction tx = DbContext.Database.BeginTransaction())
            {
                TEntity entity = DbContext.Set<TEntity>().SingleOrDefault(s => s.Id == id);
                if (entity == null)
                    return false;
                DbContext.Remove(entity);
                DbContext.SaveChanges();
                tx.Commit();
                return true;
            }
        }

		public virtual async Task RemoveAsync(Guid id, CancellationToken token = default)
		{
            DatabaseSessionController.Reset();
            using (IDbContextTransaction tx = await DbContext.Database.BeginTransactionAsync(token))
            {
                TEntity entity = await DbContext.Set<TEntity>().AsQueryable()
                    .SingleOrDefaultAsync(s => s.Id == id, token);
                if (entity == null)
                    return;
                DbContext.Remove(entity);
                await DbContext.SaveChangesAsync(token);
                await tx.CommitAsync(token);
            }
        }

		public virtual void Update(TModel model)
		{
			if (model == null)
				throw new ArgumentNullException();
            DatabaseSessionController.Reset();
            using (IDbContextTransaction tx = DbContext.Database.BeginTransaction())
            {
                TEntity entity = Mapper.Map(model);
                if (DbContext.Set<TEntity>().All(s => s.Id != entity.Id))
                    DbContext.Set<TEntity>().Add(entity);
                TEntity entry = DbContext.Set<TEntity>().FirstOrDefault(s => s.Id == entity.Id);
                if (entry != null)
                {
                    DbContext.Entry(entry).CurrentValues.SetValues(entity);
                    DbContext.SaveChanges();
                }
                tx.Commit();
            }
        }

		public virtual async Task UpdateAsync(TModel model, CancellationToken token = default)
		{
			if (model == null)
				throw new ArgumentNullException();
            DatabaseSessionController.Reset();
            using (IDbContextTransaction tx = await DbContext.Database.BeginTransactionAsync(token))
            {
                TEntity entity = Mapper.Map(model);
                if (await DbContext.Set<TEntity>().AsQueryable().AllAsync(s => s.Id != entity.Id, token)
                        .ConfigureAwait(false))
                    DbContext.Set<TEntity>().Add(entity);
                TEntity entry = await DbContext.Set<TEntity>().AsQueryable()
                    .FirstOrDefaultAsync(s => s.Id == entity.Id, token).ConfigureAwait(false);
                if (entry != null)
                {
                    DbContext.Entry(entry).CurrentValues.SetValues(entity);
                    await DbContext.SaveChangesAsync(token).ConfigureAwait(false);
                }
                await tx.CommitAsync(token);
            }
        }

        public bool Exists(Guid id)
		{
            DatabaseSessionController.Reset();
			return DbContext.Set<TEntity>().Any(s => s.Id == id);
		}

        public async Task<bool> ExistsAsync(Guid id, CancellationToken token = default)
        {
			DatabaseSessionController.Reset();
            return await DbContext.Set<TEntity>().AsQueryable().AnyAsync(s => s.Id == id, token);
		}
    }
}