using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkManager.Views.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TaskKanbanSettingsPage : ContentPage
	{
		public TaskKanbanSettingsPage()
		{
			InitializeComponent();
		}

		private void DropGestureRecognizer_Drop_Collection(object sender, DropEventArgs e)
		{
			e.Handled = true;
		}
	}
}