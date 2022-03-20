using System;
using System.Collections.Generic;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
    public class RelatedTaskModel : ModelBase, IRelatedTaskModel
    {
        public RelatedTaskModel(Guid id, string name, List<ITaskModel> relatedBy) : base(id)
        {
            Name = name;
            RelatedBy = relatedBy;
        }

        public string Name { get; set; }
        public List<ITaskModel> RelatedBy { get; set; }

        public bool Equals(IRelatedTaskModel other)
        {
            return Equals((RelatedTaskModel) other);
        }

        private bool Equals(RelatedTaskModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Equals(RelatedBy, other.RelatedBy);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((RelatedTaskModel) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, RelatedBy);
        }
    }
}