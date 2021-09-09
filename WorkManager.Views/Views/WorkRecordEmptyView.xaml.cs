using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkManager.Views.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WorkRecordEmptyView : ContentView
	{
		public WorkRecordEmptyView()
		{
			ViewModelLocator.SetAutowireViewModel(this, true);
			InitializeComponent();
		}
	}
}