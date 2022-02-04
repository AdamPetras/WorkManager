using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities.Interfaces;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface IFacade<TModel> where TModel : IModel
	{
		TModel Add(TModel model);
		Task<TModel> AddAsync(TModel model, CancellationToken token = default);
        //ICollection<TModel> GetWhere(Expression<Func<TModel, bool>> predicate);
        //Task<IEnumerable<TModel>> GetWhereAsync(Expression<Func<TModel, bool>> predicate, CancellationToken token);

        //ICollection<TModel> GetWhereOrderBy<TKey>(Expression<Func<TModel, bool>> predicate,
        //    Expression<Func<TModel, TKey>> orderBy);

        //Task<ICollection<TModel>> GetWhereOrderByAsync<TKey>(Expression<Func<TModel, bool>> predicate,
        //    Expression<Func<TModel, TKey>> orderBy, CancellationToken token);

        //ICollection<TModel> GetWhereOrderByDescending<TKey>(Expression<Func<TModel, bool>> predicate,
        //    Expression<Func<TModel, TKey>> orderBy);

        //Task<ICollection<TModel>> GetWhereOrderByDescendingAsync<TKey>(Expression<Func<TModel, bool>> predicate,
        //    Expression<Func<TModel, TKey>> orderBy, CancellationToken token);

        //int Count(Expression<Func<TModel, bool>> predicate);

        //Task<int> CountAsync(Expression<Func<TModel, bool>> predicate, CancellationToken token);

		bool Remove(Guid id);
		Task RemoveAsync(Guid id, CancellationToken token = default);
		void Update(TModel model);
		Task UpdateAsync(TModel model, CancellationToken token = default);
        ICollection<TModel> GetAll();
        Task<ICollection<TModel>> GetAllAsync(CancellationToken token = default);
		TModel GetById(Guid id);
		Task<TModel> GetByIdAsync(Guid id, CancellationToken token = default);
		bool Exists(Guid id);
		void Clear();
		Task ClearAsync (CancellationToken token = default);
	}
}