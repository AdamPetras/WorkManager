using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonServiceLocator;
using Prism.Commands;
using WorkManager.ViewModels.Pages;
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