using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Services;
using WorkManager.BL.Services;
using WorkManager.Core;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;
using Xamarin.Forms.Internals;

namespace WorkManager.ViewModels.Pages
{
	public class TaskKanbanSettingsPageViewModel : ViewModelBase
	{
		private readonly IDialogService _dialogService;
		private readonly IKanbanStateFacade _kanbanStateFacade;
		private readonly DialogEventService _dialogEventService;
		private readonly IToastMessageService _toastMessageService;
		private ITaskGroupModel _selectedTaskGroup;

		public TaskKanbanSettingsPageViewModel(INavigationService navigationService, IDialogService dialogService, IKanbanStateFacade kanbanStateFacade, 
			DialogEventService dialogEventService, IToastMessageService toastMessageService) : base(navigationService)
		{
			_dialogService = dialogService;
			_kanbanStateFacade = kanbanStateFacade;
			_dialogEventService = dialogEventService;
			_toastMessageService = toastMessageService;
			ShowCreateNewKanbanStateCommand = new DelegateCommand(async () => await ShowCreateNewKanbanState());
			SaveCommand = new DelegateCommand(async() => await Save());
			MoveItemUpCommand = new DelegateCommand<IKanbanStateModel>(ItemUp);
			MoveItemDownCommand = new DelegateCommand<IKanbanStateModel>(ItemDown);
			DeleteCommand = new DelegateCommand(Delete);
		}

		public DelegateCommand ShowCreateNewKanbanStateCommand { get; }
		public DelegateCommand DeleteCommand { get; }
		public DelegateCommand SaveCommand { get; }
		public DelegateCommand<IKanbanStateModel> MoveItemUpCommand { get; }
		public DelegateCommand<IKanbanStateModel> MoveItemDownCommand { get; }

		private ObservableCollection<IKanbanStateModel> _kanbanItems;
		private DraggableKanbanStateModel _startItemDrag;

		public ObservableCollection<IKanbanStateModel> KanbanItems
		{
			get => _kanbanItems;
			set
			{
				if (_kanbanItems == value) return;
				_kanbanItems = value;
				RaisePropertyChanged();
			}
		}

		private DraggableKanbanStateModel _selectedKanban;
		public DraggableKanbanStateModel SelectedKanban
		{
			get => _selectedKanban;
			set
			{
				if (_selectedKanban == value) return;
				_selectedKanban = value;
				RaisePropertyChanged();
			}
		}

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
			if (parameters.Any()) //ošetření navigace z dialogu
			{
				_selectedTaskGroup = parameters.GetValue<ITaskGroupModel>("TaskGroup");
				KanbanItems = new ObservableCollection<IKanbanStateModel>(_kanbanStateFacade
					.GetKanbanStatesByTaskGroup(_selectedTaskGroup.Id).Select(s => new DraggableKanbanStateModel(s)));
			}
		}

		private async Task ShowCreateNewKanbanState()
		{
			BeginProcess();
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddKanbanStateDialogView", new DialogParameters() {{"StateOrder", KanbanItems.Count}, {"TaskGroup", _selectedTaskGroup}})).Parameters;
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			if (dialogEvent != null)
			{
				_dialogEventService.OnRaiseDialogEvent(dialogEvent, KanbanItems);
				KanbanItems = new ObservableCollection<IKanbanStateModel>(KanbanItems);//neobnovuje se datatemplate
			}
			EndProcess();
		}

		private async Task Save()
		{
			if (KanbanItems.Count <= 3)
			{
				_toastMessageService.LongAlert(TranslateViewModelsSR.KanbanMustHaveThreeStates);
				return;
			}
			EnumerableDiffChecker<IKanbanStateModel> kanbanDiffChecker = new EnumerableDiffChecker<IKanbanStateModel>();
			DifferentialCollection<IKanbanStateModel> value = kanbanDiffChecker.CheckCollectionDifference(_kanbanStateFacade.GetKanbanStatesByTaskGroup(_selectedTaskGroup.Id), KanbanItems,(s,t)=>s.Id == t.Id && s.StateOrder == t.StateOrder);
			foreach (IKanbanStateModel kanbanStateModel in value.DeleteEnumerable)
			{
				await _kanbanStateFacade.RemoveAsync(kanbanStateModel.Id);
			}
			foreach (IKanbanStateModel kanbanStateModel in value.AddEnumerable)
			{
				await _kanbanStateFacade.AddAsync(kanbanStateModel);
			}
		}

		private void Delete()
		{
			if (SelectedKanban == null)
			{
				_toastMessageService.LongAlert(TranslateViewModelsSR.KanbanIsNotSelectedMessage);
				return;
			}
			KanbanItems.Remove(SelectedKanban);
			SelectedKanban = null;
			KanbanItems = new ObservableCollection<IKanbanStateModel>(KanbanItems);//neobnovuje se datatemplate
		}

		private void ItemUp(IKanbanStateModel kanbanStateModel)
		{
			int index = KanbanItems.IndexOf(kanbanStateModel);
			if (index == 0)
			{
				_toastMessageService.LongAlert(TranslateViewModelsSR.SwipeDirectionIsOutOfCollection);
				return;
			}
			KanbanItems.Remove(kanbanStateModel);
			KanbanItems.Insert(index-1,kanbanStateModel);

		}

		private void ItemDown(IKanbanStateModel kanbanStateModel)
		{
			int index = KanbanItems.IndexOf(kanbanStateModel);
			if (index == KanbanItems.Count - 1)
			{
				_toastMessageService.LongAlert(TranslateViewModelsSR.SwipeDirectionIsOutOfCollection);
				return;
			}
			KanbanItems.Remove(kanbanStateModel);
			KanbanItems.Insert(index + 1, kanbanStateModel);
		}
	}
}