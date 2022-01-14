using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities.Interfaces;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces
{
	public interface IMapper<TEntity,TModel> where TEntity:IEntity where TModel:IModel
	{
		TEntity Map(TModel model);
		TModel Map(TEntity entity);
        Task<TEntity> MapAsync(TModel model, CancellationToken token);
        Task<TModel> MapAsync(TEntity entity, CancellationToken token);
	}
}