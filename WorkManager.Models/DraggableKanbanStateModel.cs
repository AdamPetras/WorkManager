using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkManager.Models.Annotations;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class DraggableKanbanStateModel : KanbanStateModel, IDraggableModel, INotifyPropertyChanged
	{
		public DraggableKanbanStateModel(Guid id, string name, int stateOrder, string iconName, ITaskGroupModel taskGroup) : base(id, name, stateOrder, iconName, taskGroup)
		{

		}

		public DraggableKanbanStateModel(IKanbanStateModel kanbanStateModel) : base (kanbanStateModel.Id, kanbanStateModel.Name, kanbanStateModel.StateOrder, kanbanStateModel.IconName, kanbanStateModel.TaskGroup)
		{
			
		}

		private bool _isBeingDragged;
		public bool IsBeingDragged
		{
			get => _isBeingDragged;
			set
			{
				if (_isBeingDragged == value) return;
				_isBeingDragged = value;
				OnPropertyChanged();
			}
		}

		private bool _isBeingDraggedOverFromTop;
		public bool IsBeingDraggedOverFromTop
		{
			get => _isBeingDraggedOverFromTop;
			set
			{
				if (_isBeingDraggedOverFromTop == value) return;
				_isBeingDraggedOverFromTop = value;
				OnPropertyChanged();
			}
		}

		private bool _isBeingDraggedOverFromBottom;
		public bool IsBeingDraggedOverFromBottom
		{
			get => _isBeingDraggedOverFromBottom;
			set
			{
				if (_isBeingDraggedOverFromBottom == value) return;
				_isBeingDraggedOverFromBottom = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}