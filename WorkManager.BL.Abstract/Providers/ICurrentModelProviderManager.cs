using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Providers
{
	public interface ICurrentModelProviderManager<TModel> : ICurrentModelProvider<TModel> where TModel : IModel
	{
		void SetItem(TModel item);
	}
}