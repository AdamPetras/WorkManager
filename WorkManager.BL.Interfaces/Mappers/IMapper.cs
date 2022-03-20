using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities.Interfaces;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Mappers
{
	public interface IMapper<out TEntity, in TModel> where TModel:IModel where TEntity : IEntity
    {
        TEntity Map(TModel model);
	}
}