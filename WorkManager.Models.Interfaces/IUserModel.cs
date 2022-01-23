using System;
using System.ComponentModel.DataAnnotations;

namespace WorkManager.Models.Interfaces
{
	public interface IUserModel : IModel, IEquatable<IUserModel>
	{
        [StringLength(30)]
		string FirstName { get; set; }
        [StringLength(30)]
        string Surname { get; set; }
        [StringLength(20)]
        string Username { get; set; }
        [StringLength(20)]
		string Password { get; set; }
	}
}