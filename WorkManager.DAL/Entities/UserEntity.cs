using System.Collections.Generic;
using WorkManager.DAL.Entities.BaseClasses;

namespace WorkManager.DAL.Entities
{
	public class UserEntity : EntityBase
	{
		public UserEntity()
		{
		}

		public string Username { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string Surname { get; set; }
	}
}