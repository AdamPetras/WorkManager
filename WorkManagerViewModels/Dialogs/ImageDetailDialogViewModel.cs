using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Dialogs
{
	public class ImageDetailDialogViewModel : DialogViewModelBase
	{
		public ImageDetailDialogViewModel(INavigationService navigationService) : base(navigationService)
		{
			CloseCommand = new DelegateCommand(() => OnRequestClose(null));
		}

		public DelegateCommand CloseCommand { get; }

		private string _path;
		public string Path
		{
			get => _path;
			set
			{
				if (_path == value) return;
				_path = value;
				RaisePropertyChanged();
			}
		}

		protected override void OnDialogOpenedInt(IDialogParameters parameters)
		{
			base.OnDialogOpenedInt(parameters);
			Path = parameters.GetValue<string>("Path");
		}
	}
}