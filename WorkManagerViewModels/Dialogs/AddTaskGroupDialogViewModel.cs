﻿using System;
using System.Linq;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.Enums;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Dialogs
{
	public class AddTaskGroupDialogViewModel : DialogViewModelBase
	{
		private readonly ITaskGroupFacade _taskGroupFacade;
		private readonly IKanbanStateFacade _kanbanStateFacade;
		private readonly ICurrentModelProvider<IUserModel> _currentUserProvider;
		private readonly IEventAggregator _eventAggregator;
		private readonly IToastMessageService _toastMessageService;

		public AddTaskGroupDialogViewModel(INavigationService navigationService, ITaskGroupFacade taskGroupFacade, IKanbanStateFacade kanbanStateFacade, ICurrentModelProvider<IUserModel> currentUserProvider,
			IEventAggregator eventAggregator, IToastMessageService toastMessageService) : base(navigationService)
		{
			_taskGroupFacade = taskGroupFacade;
			_kanbanStateFacade = kanbanStateFacade;
			_currentUserProvider = currentUserProvider;
			_eventAggregator = eventAggregator;
			_toastMessageService = toastMessageService;
			CancelCommand = new DelegateCommand(Cancel);
			ConfirmCommand = new DelegateCommand(Confirm);
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
		private void Cancel()
		{
			OnRequestClose(null);
		}

		private void Confirm()
		{
			if (_taskGroupFacade.GetAll().Any(s => s.Name == Name))
			{
				_toastMessageService.LongAlert(TranslateViewModelsSR.TaskGroupNameAlreadyExistsFormat(Name));
				Cancel();
				return;
			}
			ITaskGroupModel model = new TaskGroupModel(Guid.NewGuid(), Name, Description, _currentUserProvider.GetModel());
			_taskGroupFacade.Add(model);
			_kanbanStateFacade.CreateDefaultKanbanStateModels(model);
			OnRequestClose(new DialogParameters(){{ "DialogEvent", new AddAfterDialogCloseDialogEvent<ITaskGroupModel>(model) } });
		}
	}
}