using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.DAL.Enums;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Dialogs
{
	public class FilterDialogViewModel:DialogViewModelBase
	{
		public FilterDialogViewModel(INavigationService navigationService) : base(navigationService)
		{
			ConfirmCommand = new DelegateCommand(Confirm);
			CancelCommand = new DelegateCommand(Cancel);
		}

		public DelegateCommand ConfirmCommand { get; }
		public DelegateCommand CancelCommand { get; }
		
		private EFilterType _selectedFilter;
		public EFilterType SelectedFilter
		{
			get => _selectedFilter;
			set
			{
				if (_selectedFilter == value) return;
				_selectedFilter = value;
				RaisePropertyChanged();
			}
		}


		protected override void OnDialogOpenedInt(IDialogParameters parameters)
		{
			base.OnDialogOpenedInt(parameters);
			_selectedFilter = parameters.GetValue<EFilterType>("Filter");
			RaisePropertyChanged(nameof(SelectedFilter));
		}

		private void Confirm()
		{
			OnRequestClose(new DialogParameters() { { "Filter", _selectedFilter } });
		}

		private void Cancel()
		{
			OnRequestClose(null);
		}
	}
}