using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.Entities
{
	public class UserEntity : IEntity
	{
		public UserEntity()
		{
		}
        [Key]
        public Guid Id { get; set; }
        [StringLength(20)]
		public string Username { get; set; }
        [StringLength(20)]
        public string Password { get; set; }
        [StringLength(30)]
		public string FirstName { get; set; }
        [StringLength(30)]
		public string Surname { get; set; }
	}
}