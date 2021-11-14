using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface IRepository<TEntity> where TEntity : IEntity
	{
        ICollection<TEntity> GetAll();
		Task<ICollection<TEntity>> GetAllAsync(CancellationToken token);
		TEntity GetById(Guid id);
		Task<TEntity> GetByIdAsync(Guid id, CancellationToken token);
		bool Remove(Guid id);
		Task<bool> RemoveAsync(Guid id, CancellationToken token);
		TEntity Add(TEntity entity);
		Task<TEntity> AddAsync(TEntity entity, CancellationToken token);
		void Update(TEntity entity);
		Task UpdateAsync(TEntity entity, CancellationToken token);
		bool Exists(TEntity entity);
		Task<bool> ExistsAsync(TEntity entity, CancellationToken token);
		void Clear();
		Task ClearAsync(CancellationToken token);
	}
}