using System;
using System.Collections.Generic;
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
        IEnumerable<TModel> GetAll();
		Task<IEnumerable<TModel>> GetAllAsync(CancellationToken token = default);
		TModel GetById(Guid id);
		Task<TModel> GetByIdAsync(Guid id, CancellationToken token = default);
		bool ModelExists(Guid id);
		void Clear();
		Task ClearAsync (CancellationToken token = default);
	}
}