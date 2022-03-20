using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WorkManager.DAL.Enums;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;
using WorkManager.Xamarin.Core;

namespace WorkManager.Models
{
    public class TaskModel : ModelBase, ITaskModel
    {
        public TaskModel() : base(Guid.Empty)
        {
        }

        public TaskModel([NotNull] ITaskModel task) : this(task.Id, task.ActualDateTime, task.Name, task.ImagesCount, task.Description, task.TaskDoneDateTime, task.TaskGroupId, task.StateId, task.Priority.GetValue<EPriority>(), task.WorkTime, task.RelatedTaskId)
        {

        }

        public TaskModel(Guid id, DateTime actualDateTime, string name, int imagesCount, string description, DateTime taskDoneDateTime,
            Guid taskGroupId, Guid stateId, EPriority priority, TimeSpan workTime, Guid relatedTaskId) : base(id)
        {
            ActualDateTime = actualDateTime;
            Name = name;
            ImagesCount = imagesCount;
            Description = description;
            TaskDoneDateTime = taskDoneDateTime;
            TaskGroupId = taskGroupId;
            StateId = stateId;
            Priority = new LocalizedEnum(priority);
            WorkTime = workTime;
            RelatedTaskId = relatedTaskId;
        }

        public DateTime ActualDateTime { get; set; }
        public string Name { get; set; }
        public int ImagesCount { get; set; }
        public string Description { get; set; }
        public DateTime TaskDoneDateTime { get; set; }
        public Guid TaskGroupId { get; set; }
        public Guid StateId { get; set; }
        public LocalizedEnum Priority { get; set; }
        public TimeSpan WorkTime { get; set; }
        public Guid RelatedTaskId { get; set; }


        public bool Equals(ITaskModel other)
        {
            return Equals((TaskModel)other);
        }

        private bool Equals(TaskModel other)
        {
            return ActualDateTime.Equals(other.ActualDateTime) && Name == other.Name && Description == other.Description &&
                   TaskDoneDateTime.Equals(other.TaskDoneDateTime) && TaskGroupId == other.TaskGroupId &&
                   StateId == other.StateId && Priority == other.Priority && WorkTime.Equals(other.WorkTime) && RelatedTaskId == other.RelatedTaskId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TaskModel)obj);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(ActualDateTime);
            hash.Add(Name);
            hash.Add(ImagesCount);
            hash.Add(Description);
            hash.Add(TaskDoneDateTime);
            hash.Add(TaskGroupId);
            hash.Add(StateId);
            hash.Add((int)Priority.GetValue<EPriority>());
            hash.Add(WorkTime);
            hash.Add(RelatedTaskId);
            return hash.ToHashCode();
        }
    }
}
