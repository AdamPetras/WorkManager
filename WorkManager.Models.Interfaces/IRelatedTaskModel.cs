using System;
using System.Collections.Generic;

namespace WorkManager.Models.Interfaces
{
    public interface IRelatedTaskModel : IModel, IEquatable<IRelatedTaskModel>
    {
        string Name { get; set; }
        List<ITaskModel> RelatedBy {get; set; }
    }
}