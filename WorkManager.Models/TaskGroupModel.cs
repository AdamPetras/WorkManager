using System;
using System.Collections.Generic;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
    public class TaskGroupModel : ModelBase, ITaskGroupModel
    {

        public TaskGroupModel() : base(Guid.Empty)
        {

        }

        public TaskGroupModel(Guid id, string name, string description, int tasksCount, Guid userId) : base(id)
        {
            Name = name;
            Description = description;
            TasksCount = tasksCount;
            UserId = userId;
        }

        public string Name { get; set; }
        public string Description { get; set; }

        private int _tasksCount;
        public int TasksCount
        {
            get => _tasksCount;
            set
            {
                _tasksCount = value;
                RaisePropertyChanged();
            }
        }
        public Guid UserId { get; set; }

        public bool Equals(ITaskGroupModel other)
        {
            return Equals((TaskGroupModel)other);
        }

        protected bool Equals(TaskGroupModel other)
        {
            return Name == other.Name && Description == other.Description && UserId == other.UserId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TaskGroupModel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description, UserId);
        }
    }
}