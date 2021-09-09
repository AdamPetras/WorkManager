using System;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class UserModel : ModelBase, IUserModel
	{
		public UserModel() : base(Guid.Empty)
		{

		}

		public UserModel(Guid id, string firstName, string surname, string username, string password) : base(id)
		{
			FirstName = firstName;
			Username = username;
			Password = password;
			Surname = surname;
		}

		public string FirstName { get; set; }
		public string Surname { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}
}