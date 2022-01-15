using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkManager.Models.Annotations;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models.BaseClasses
{
	public abstract class ModelBase:IModel, INotifyPropertyChanged
	{
		protected ModelBase(Guid id)
		{
			Id = id;
		}
		public Guid Id { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}