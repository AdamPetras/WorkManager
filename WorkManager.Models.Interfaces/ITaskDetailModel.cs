using System;
using System.Collections.Generic;

namespace WorkManager.Models.Interfaces
{
    public interface ITaskDetailModel : ITaskModel, IEquatable<ITaskDetailModel>
    {
        public List<IRelatedTaskModel> RelatedTasks { get; set; }
    }
}