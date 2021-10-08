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
using WorkManager.BL.Services;
using WorkManager.Core;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using Xamarin.Forms.Internals;

namespace WorkManager.ViewModels.Pages
{
	public class TaskKanbanSettingsPageViewModel : ViewModelBase
	{
		private readonly IDialogService _dialogService;
		private readonly IKanbanStateFacade _kanbanStateFacade;
		private readonly IKanbanTaskGroupFacade _kanbanTaskGroupFacade;
		private readonly DialogEventService _dialogEventService;
		private ITaskGroupModel _selectedTaskGroup;

		public TaskKanbanSettingsPageViewModel(INavigationService navigationService, IDialogService dialogService, IKanbanStateFacade kanbanStateFacade, IKanbanTaskGroupFacade kanbanTaskGroupFacade, DialogEventService dialogEventService) : base(navigationService)
		{
			_dialogService = dialogService;
			_kanbanStateFacade = kanbanStateFacade;
			_kanbanTaskGroupFacade = kanbanTaskGroupFacade;
			_dialogEventService = dialogEventService;
			ItemDraggedCommand = new DelegateCommand<DraggableKanbanStateModel>(ItemDragged);
			ItemDroppedCommand = new DelegateCommand<DraggableKanbanStateModel>(ItemDropped);
			ItemDraggedOverCommand = new DelegateCommand<DraggableKanbanStateModel>(ItemDraggedOver);
			ItemDragLeaveCommand = new DelegateCommand<DraggableKanbanStateModel>(ItemDragLeave);
			ShowCreateNewKanbanStateCommand = new DelegateCommand(async () => await ShowCreateNewKanbanState());
			SaveCommand = new DelegateCommand(async() => await Save());
		}

		public DelegateCommand ShowCreateNewKanbanStateCommand { get; }
		public DelegateCommand<DraggableKanbanStateModel> ItemDraggedCommand { get; }
		public DelegateCommand<DraggableKanbanStateModel> ItemDroppedCommand { get; }
		public DelegateCommand<DraggableKanbanStateModel> ItemDraggedOverCommand { get; }
		public DelegateCommand<DraggableKanbanStateModel> ItemDragLeaveCommand { get; }
		public DelegateCommand SaveCommand { get; }

		private ObservableCollection<DraggableKanbanStateModel> _kanbanItems;
		private DraggableKanbanStateModel _startItemDrag;

		public ObservableCollection<DraggableKanbanStateModel> KanbanItems
		{
			get => _kanbanItems;
			set
			{
				if (_kanbanItems == value) return;
				_kanbanItems = value;
				RaisePropertyChanged();
			}
		}


		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
			if (parameters.Any()) //ošetření navigace z dialogu
			{
				_selectedTaskGroup = parameters.GetValue<ITaskGroupModel>("TaskGroup");
				KanbanItems = new ObservableCollection<DraggableKanbanStateModel>(_kanbanStateFacade
					.GetKanbanStateByTaskGroup(_selectedTaskGroup.Id).Select(s => new DraggableKanbanStateModel(s)));
			}
		}

		private void ItemDragged(DraggableKanbanStateModel item)
		{
			_startItemDrag = item;
		}

		private void ItemDraggedOver(DraggableKanbanStateModel item)
		{
			if (item.Name == _startItemDrag.Name)
				return;
			if (item.StateOrder < _startItemDrag.StateOrder)
				item.IsBeingDraggedOverFromBottom = true;
			else
				item.IsBeingDraggedOverFromTop = true;
		}

		private void ItemDragLeave(DraggableKanbanStateModel item)
		{
			KanbanItems.ForEach(i =>
			{
				i.IsBeingDraggedOverFromBottom = false;
				i.IsBeingDraggedOverFromTop = false;
			});
		}

		private void ItemDropped(DraggableKanbanStateModel item)
		{
			DraggableKanbanStateModel itemToMove = _startItemDrag;
			DraggableKanbanStateModel itemToInsertAfter = item;
			if (itemToMove == null || itemToInsertAfter == null || itemToMove.Name == itemToInsertAfter.Name)
				return;
			KanbanItems.Remove(itemToMove);
			int insertAtIndex = KanbanItems.IndexOf(itemToInsertAfter);
			//index je podmíněn kvůli tom, že je rozdíl pokud beru položku ze spod a vkládám nahoru či beru položku ze zhora a vkládám do spod
			KanbanItems.Insert(insertAtIndex + (itemToInsertAfter.IsBeingDraggedOverFromTop ? 1 : 0), itemToMove);
			itemToMove.IsBeingDragged = false;
			itemToInsertAfter.IsBeingDraggedOverFromBottom = false;
			itemToInsertAfter.IsBeingDraggedOverFromTop = false;
			//nastavení indexů každého prvku musím každý prvek, protože když vložím položku ze zhora do středu tak budu muset změnit indexy všech položek předtím
			KanbanItems.ForEach(s => s.StateOrder = KanbanItems.IndexOf(s));
		}

		private async Task ShowCreateNewKanbanState()
		{
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddKanbanStateDialogView")).Parameters;
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			DraggableKanbanStateModel model = KanbanItems.Last();
			KanbanItems.Remove(KanbanItems.Last());		//odstraním poslední prvek přidám poslední a následně vložím prvek na předposlední místo byl zde problém s obnovou datatemplate
			_dialogEventService.OnRaiseDialogEvent(dialogEvent, KanbanItems);
			KanbanItems.Insert(KanbanItems.Count - 1, model);
		}

		private async Task Save()
		{
			EnumerableDiffChecker<IKanbanStateModel> kanbanDiffChecker = new EnumerableDiffChecker<IKanbanStateModel>();
			DifferentialCollection<IKanbanStateModel> value = kanbanDiffChecker.CheckCollectionDifference(_kanbanStateFacade.GetKanbanStateByTaskGroup(_selectedTaskGroup.Id), KanbanItems,(s,t)=>s.Id == t.Id);
			foreach (IKanbanStateModel kanbanStateModel in value.AddEnumerable)
			{
				await _kanbanStateFacade.AddAsync(kanbanStateModel);
				await _kanbanTaskGroupFacade.AddAsync(new KanbanTaskGroupModel(Guid.NewGuid(), _selectedTaskGroup,
					kanbanStateModel));
			}
			foreach (IKanbanStateModel kanbanStateModel in value.DeleteEnumerable)
			{
				await _kanbanStateFacade.RemoveAsync(kanbanStateModel.Id);
			}
			foreach (IKanbanStateModel kanbanStateModel in value.UpdateEnumerable)
			{
				await _kanbanStateFacade.UpdateAsync(kanbanStateModel);
			}
		}
	}
}