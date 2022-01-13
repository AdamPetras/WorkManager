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
		public string Username { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string Surname { get; set; }
	}
}