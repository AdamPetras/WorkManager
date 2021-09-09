using WorkManager.DAL.Entities.Interfaces;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces
{
	public interface IMapper<TEntity,TModel> where TEntity:IEntity where TModel:IModel
	{
		TEntity Map(TModel model);
		TModel Map(TEntity entity);
	}
}