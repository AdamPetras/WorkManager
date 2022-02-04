using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface IRepository<TEntity> where TEntity : IEntity
	{
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token);
        IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);

        IEnumerable<TEntity> GetWhereOrderBy<TKey>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderBy);

        Task<IEnumerable<TEntity>> GetWhereOrderByAsync<TKey>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderBy, CancellationToken token);

        IEnumerable<TEntity> GetWhereOrderByDescending<TKey>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderBy);

        Task<IEnumerable<TEntity>> GetWhereOrderByDescendingAsync<TKey>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderBy, CancellationToken token);

        int Count(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);
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
        bool Exists(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);
        void Clear();
		Task ClearAsync(CancellationToken token);
	}
}