using System;
using System.Linq;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Dialogs
{
	public class AddTaskDialogViewModel : DialogViewModelBase
	{
		private readonly ITaskFacade _taskFacade;
		private readonly ICurrentModelProvider<ITaskGroupModel> _currentTaskGroupProvider;
		private readonly IKanbanTaskGroupFacade _kanbanTaskGroupFacade;
		private readonly IEventAggregator _eventAggregator;

		public AddTaskDialogViewModel(INavigationService navigationService, ICurrentModelProvider<ITaskGroupModel> currentTaskGroupProvider, IKanbanTaskGroupFacade kanbanTaskGroupFacade, ITaskFacade taskFacade, IEventAggregator eventAggregator) : base(navigationService)
		{
			_currentTaskGroupProvider = currentTaskGroupProvider;
			_kanbanTaskGroupFacade = kanbanTaskGroupFacade;
			_eventAggregator = eventAggregator;
			_taskFacade = taskFacade;
			CancelCommand = new DelegateCommand(Cancel);
			ConfirmCommand = new DelegateCommand(Confirm);
			TaskStartDate = DateTime.Now;
			TaskDoneDate = DateTime.Now;
		}

		public DelegateCommand CancelCommand { get; }
		public DelegateCommand ConfirmCommand { get; }

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

		private string _description;
		public string Description
		{
			get => _description;
			set
			{
				if (_description == value) return;
				_description = value;
				RaisePropertyChanged();
			}
		}

		private DateTime _taskStartDate;
		public DateTime TaskStartDate
		{
			get => _taskStartDate;
			set
			{
				if (_taskStartDate == value) return;
				_taskStartDate = value;
				RaisePropertyChanged();
			}
		}

		private DateTime _taskDoneDate;
		public DateTime TaskDoneDate
		{
			get => _taskDoneDate;
			set
			{
				if (_taskDoneDate == value) return;
				_taskDoneDate = value;
				RaisePropertyChanged();
			}
		}

		private void Cancel()
		{
			OnRequestClose(null);
		}

		private void Confirm()
		{
			IKanbanStateModel firstKanban = _kanbanTaskGroupFacade
				.GetKanbansByTaskGroupId(_currentTaskGroupProvider.GetModel().Id).Single(s => s.Kanban.StateOrder == 0)
				.Kanban;
			ITaskModel model = new TaskModel(Guid.NewGuid(), TaskStartDate, Name, Description, TaskDoneDate,
				_currentTaskGroupProvider.GetModel(), firstKanban);
			_taskFacade.Add(model);
			OnRequestClose(new DialogParameters(){{ "DialogEvent", new AddAfterDialogCloseDialogEvent<ITaskModel>(model)} });
		}
	}
}