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
		bool Remove(Guid id);
		Task RemoveAsync(Guid id, CancellationToken token = default);
		void Update(TModel model);
		Task UpdateAsync(TModel model, CancellationToken token = default);
		bool Exists(Guid id);
		Task<bool> ExistsAsync(Guid id, CancellationToken token = default);
    }
}