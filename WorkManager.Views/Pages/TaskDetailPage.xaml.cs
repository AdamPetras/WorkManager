using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkManager.Views.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TaskDetailPage : ContentPage
	{
		public TaskDetailPage()
		{
			InitializeComponent();
		}
	}
}