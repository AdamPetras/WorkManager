using System.Collections;
using Xamarin.Forms;

namespace WorkManager.Views.TemplateSelectors
{
	public class LastItemOfCollectionTemplateSelector : DataTemplateSelector
	{
		public DataTemplate DefaultDataTemplate { get; set; }
		public DataTemplate LastItemDataTemplate { get; set; }

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			return ((IList)((CollectionView)container).ItemsSource).IndexOf(item) == ((IList)((CollectionView)container).ItemsSource).Count-1 ? LastItemDataTemplate : DefaultDataTemplate;
		}
	}
}