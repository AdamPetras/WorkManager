using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.NavigationParams;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Dialogs
{
	public class AddKanbanStateDialogViewModel : ConfirmDialogViewModelBase
	{
		private ITaskGroupModel _selectedTaskGroup;
		private int _selectedStateOrder;

		public AddKanbanStateDialogViewModel(INavigationService navigationService) : base(navigationService)
		{
			IsIconSelectionVisible = false;
			ShowHideSelectionCommand = new DelegateCommand(() => { IsIconSelectionVisible = !IsIconSelectionVisible; });
			SelectionChangedCommand = new DelegateCommand(() => IsIconSelectionVisible = false);
		}

		public DelegateCommand ShowHideSelectionCommand { get; }
		public DelegateCommand SelectionChangedCommand { get; }

        private string _selectedIcon;
		public string SelectedIcon
		{
			get => _selectedIcon;
			set
			{
				if (_selectedIcon == value) return;
				_selectedIcon = value;
				RaisePropertyChanged();
			}
		}

		private bool _isIconSelectionVisible;
		public bool IsIconSelectionVisible
		{
			get => _isIconSelectionVisible;
			set
			{
				if (_isIconSelectionVisible == value) return;
				_isIconSelectionVisible = value;
				RaisePropertyChanged();
			}
		}

		private string _name;

		public string Name
		{
			get => _name;
			set
			{
				if (_name == value) return;
				_name = value;
				RaisePropertyChanged();
			}
		}

		protected override void OnDialogOpenedInt(IDialogParameters parameters)
		{
			base.OnDialogOpenedInt(parameters);
            StateOrderTaskGroupNavigationParameters navParameters = new StateOrderTaskGroupNavigationParameters(parameters);
            _selectedStateOrder = navParameters.StateOrder;
            _selectedTaskGroup = navParameters.TaskGroup;
        }

#pragma warning disable CS1998
        protected override async Task ConfirmAsyncInt()
#pragma warning restore CS1998
        {
			IKanbanStateModel stateModel = new KanbanStateModel(Guid.NewGuid(), Name, _selectedStateOrder, SelectedIcon, _selectedTaskGroup.Id);
            OnRequestClose(new DialogParameters() { { "DialogEvent", new AddAfterDialogCloseDialogEvent<IKanbanStateModel>(stateModel) } });
		}
	}
}