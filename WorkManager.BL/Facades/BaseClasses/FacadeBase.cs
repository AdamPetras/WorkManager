using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.Entities.Interfaces;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades.BaseClasses
{
	public abstract class FacadeBase<TModel, TEntity> : IFacade<TModel> where TModel : IModel where TEntity : IEntity
	{
        protected IRepository<TEntity> Repository;
		protected readonly IMapper<TEntity, TModel> Mapper;

		protected FacadeBase(IRepository<TEntity> repository, IMapper<TEntity, TModel> mapper, IDatabaseSessionController databaseSessionController)
		{
            DatabaseSessionController = databaseSessionController;
            Repository = repository;
			Mapper = mapper;
		}

        public IDatabaseSessionController DatabaseSessionController { get; }

		public virtual TModel Add(TModel model)
		{
			if (model == null)
				throw new ArgumentNullException();
            DatabaseSessionController.Reset();
			if (Repository.Add(Mapper.Map(model)) == null)
				return default;
			return model;
		}

		public virtual async Task<TModel> AddAsync(TModel model, CancellationToken token = default)
		{
			if (model == null)
                throw new ArgumentNullException();
            DatabaseSessionController.Reset();
			if (await Repository.AddAsync(await Mapper.MapAsync(model, token), token).ConfigureAwait(false) == null)
			{
				return default;
			}
			return model;
		}

        public virtual bool Remove(Guid id)
		{
            DatabaseSessionController.Reset();
			return Repository.Remove(id);
		}

		public virtual async Task RemoveAsync(Guid id, CancellationToken token = default)
		{
            DatabaseSessionController.Reset();
			await Repository.RemoveAsync(id, token).ConfigureAwait(false);
		}

		public virtual void Update(TModel model)
		{
			if (model == null)
				throw new ArgumentNullException();
            DatabaseSessionController.Reset();
			Repository.Update(Mapper.Map(model));
		}

		public virtual async Task UpdateAsync(TModel model, CancellationToken token = default)
		{
			if (model == null)
				throw new ArgumentNullException();
            DatabaseSessionController.Reset();
			await Repository.UpdateAsync(await Mapper.MapAsync(model, token), token).ConfigureAwait(false);
		}

		public ICollection<TModel> GetAll()
		{
            DatabaseSessionController.Reset();
			return Repository.GetAll().Select(Mapper.Map).ToList();
		}

		public async Task<ICollection<TModel>> GetAllAsync(CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return (await Repository.GetAllAsync(token).ConfigureAwait(false)).Select(Mapper.Map).ToList();
        }

		public TModel GetById(Guid id)
		{
            DatabaseSessionController.Reset();
			TEntity tmp = Repository.GetById(id);
			return tmp != null ? Mapper.Map(tmp) : default;
		}

		public async Task<TModel> GetByIdAsync(Guid id, CancellationToken token = default)
		{
            DatabaseSessionController.Reset();
			TEntity tmp = await Repository.GetByIdAsync(id, token).ConfigureAwait(false);
			return tmp != null ? await Mapper.MapAsync(tmp, token) : default;
		}

		public bool Exists(Guid id)
		{
            DatabaseSessionController.Reset();
			return Repository.GetAll().Any(s => Mapper.Map(s).Id == id);
		}

		public void Clear()
		{
            DatabaseSessionController.Reset();
			Repository.Clear();
		}

		public async Task ClearAsync(CancellationToken token = default)
		{
            DatabaseSessionController.Reset();
			await Repository.ClearAsync(token).ConfigureAwait(false);
		}
	}
}