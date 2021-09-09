using WorkManager.BL.Interfaces.Providers;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Providers
{
	public class CurrentModelProvider<T>:ICurrentModelProvider<T>, ICurrentModelProviderManager<T> where T:IModel
	{
		private T _model;
		public T GetModel()
		{
			return _model;
		}

		public void SetItem(T item)
		{
			_model = item;
		}
	}
}