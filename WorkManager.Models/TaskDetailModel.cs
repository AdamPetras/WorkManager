using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WorkManager.DAL.Enums;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
    public class TaskDetailModel : TaskModel, ITaskDetailModel
    {
        public TaskDetailModel()
        {
            
        }

        public TaskDetailModel([NotNull] ITaskDetailModel task) : this(task.Id, task.ActualDateTime, task.Name, task.ImagesCount, task.Description, task.TaskDoneDateTime, task.TaskGroupId, task.StateId, task.Priority.GetValue<EPriority>(), task.WorkTime, task.RelatedTasks)
        {

        }

        public TaskDetailModel(Guid id, DateTime actualDateTime, string name, int imagesCount, string description, DateTime taskDoneDateTime,
            Guid taskGroupId, Guid stateId, EPriority priority, TimeSpan workTime, List<IRelatedTaskModel> relatedTasks) : base(id,actualDateTime,name,imagesCount,description, taskDoneDateTime, taskGroupId, stateId, priority, workTime)
        {
            RelatedTasks = relatedTasks;
        }

        public List<IRelatedTaskModel> RelatedTasks { get; set; }

        public bool Equals(ITaskDetailModel other)
        {
            return Equals((object)other);
        }

        private bool Equals(TaskDetailModel other)
        {
            return base.Equals(other) && RelatedTasks.SequenceEqual(other.RelatedTasks);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TaskDetailModel) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), RelatedTasks);
        }
    }
}