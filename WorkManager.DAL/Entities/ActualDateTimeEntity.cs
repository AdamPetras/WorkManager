using System;
using System.ComponentModel.DataAnnotations;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.Entities
{
    public class ActualDateTimeEntity : IEntity
    {
        public ActualDateTimeEntity()
        {
        }
        [Key]
        public Guid Id { get; set; }
        public DateTime ActualDateTime { get; set; }
    }
}