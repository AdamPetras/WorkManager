using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface IRepository<TEntity> where TEntity : IEntity
	{
		IEnumerable<TEntity> GetAll();
		Task<List<TEntity>> GetAllAsync(CancellationToken token = default);
		TEntity GetById(Guid id);
		Task<TEntity> GetByIdAsync(Guid id, CancellationToken token = default);
		bool Remove(Guid id);
		Task<bool> RemoveAsync(Guid id, CancellationToken token = default);
		TEntity Add(TEntity entity);
		Task<TEntity> AddAsync(TEntity entity, CancellationToken token = default);
		void Update(TEntity entity);
		Task UpdateAsync(TEntity entity, CancellationToken token = default);
		bool Exists(TEntity entity);
		Task<bool> ExistsAsync(TEntity entity, CancellationToken token = default);
		void Clear();
	}
}