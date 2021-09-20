using System;

namespace WorkManager.Models.Interfaces
{
	public interface IUserModel : IModel, IEquatable<IUserModel>
	{
		string FirstName { get; set; }
		string Surname { get; set; }
		string Username { get; set; }
		string Password { get; set; }
	}
}