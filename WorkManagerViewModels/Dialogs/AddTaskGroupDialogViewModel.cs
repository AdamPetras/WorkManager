using System;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Core;
using WorkManager.DAL.Enums;
using WorkManager.Extensions;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Dialogs
{
	public class AddTaskGroupDialogViewModel : ConfirmDialogViewModelBase
	{
		private readonly ITaskGroupFacade _taskGroupFacade;
		private readonly IKanbanStateFacade _kanbanStateFacade;
		private readonly ICurrentModelProvider<IUserModel> _currentUserProvider;
		private readonly IToastMessageService _toastMessageService;

		public AddTaskGroupDialogViewModel(INavigationService navigationService, ITaskGroupFacade taskGroupFacade,
            IKanbanStateFacade kanbanStateFacade, ICurrentModelProvider<IUserModel> currentUserProvider, IToastMessageService toastMessageService,
            ViewModelTaskExecute viewModelTaskExecute) : base(navigationService, viewModelTaskExecute)
		{
			_taskGroupFacade = taskGroupFacade;
			_kanbanStateFacade = kanbanStateFacade;
			_currentUserProvider = currentUserProvider;
			_toastMessageService = toastMessageService;
            NameMaxLength = typeof(ITaskGroupModel).GetStringMaxLength(nameof(ITaskGroupModel.Name));
		}

		public int NameMaxLength { get; }

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
		
		protected override async Task ConfirmAsyncInt()
        {
			BeginProcess();
            if (await ViewModelTaskExecute.ExecuteTaskWithQueue(Name, _taskGroupFacade.ExistsAsync))
            {
                _toastMessageService.LongAlert(TranslateViewModelsSR.TaskGroupNameAlreadyExists.Format(Name));
                CancelInt();
                return;
            }
            ITaskGroupModel model = new TaskGroupModel(Guid.NewGuid(), Name, Description, 0, _currentUserProvider.GetModel().Id);
            await ViewModelTaskExecute.ExecuteTaskWithQueue(async token =>
            {
                await _taskGroupFacade.AddAsync(model,token);
                await _kanbanStateFacade.CreateDefaultKanbanStateModelsAsync(model, token);
			});
            OnRequestClose(new DialogParameters() { { "DialogEvent", new AddAfterDialogCloseDialogEvent<ITaskGroupModel>(model) } });
            EndProcess();
		}
    }
}