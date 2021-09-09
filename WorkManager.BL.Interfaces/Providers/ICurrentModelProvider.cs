using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Providers
{
	public interface ICurrentModelProvider<out TModel> : IProvider where TModel : IModel
	{
		TModel GetModel();
	}
}