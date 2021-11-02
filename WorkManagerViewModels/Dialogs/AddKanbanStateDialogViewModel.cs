using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Dialogs
{
	public class AddKanbanStateDialogViewModel : DialogViewModelBase
	{
		private ITaskGroupModel _selectedTaskGroup;
		private int _selectedStateOrder;

		public AddKanbanStateDialogViewModel(INavigationService navigationService) : base(navigationService)
		{
			IsIconSelectionVisible = false;
			ShowHideSelectionCommand = new DelegateCommand(() => { IsIconSelectionVisible = !IsIconSelectionVisible; });
			SelectionChangedCommand = new DelegateCommand(() => IsIconSelectionVisible = false);
			CancelCommand = new DelegateCommand(()=> OnRequestClose(null));
			ConfirmCommand = new DelegateCommand(async () => await Confirm());
		}

		public DelegateCommand ShowHideSelectionCommand { get; }
		public DelegateCommand SelectionChangedCommand { get; }
		public DelegateCommand CancelCommand { get; }
		public DelegateCommand ConfirmCommand { get; }

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
			_selectedStateOrder = parameters.GetValue<int>("StateOrder");
			_selectedTaskGroup = parameters.GetValue<ITaskGroupModel>("TaskGroup");
		}

		private async Task Confirm()
		{
			DraggableKanbanStateModel stateModel = new DraggableKanbanStateModel(Guid.NewGuid(), Name, _selectedStateOrder, SelectedIcon,_selectedTaskGroup);
			OnRequestClose(new DialogParameters(){{ "DialogEvent", new AddAfterDialogCloseDialogEvent<DraggableKanbanStateModel>(stateModel) } });
		}
	}
}