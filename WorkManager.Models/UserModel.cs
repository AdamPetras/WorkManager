using System;
using WorkManager.Core.Annotations;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class UserModel : ModelBase, IUserModel
	{
		public UserModel() : base(Guid.Empty)
		{

		}

        public UserModel([NotNull]IUserModel user) :this(user.Id,user.FirstName,user.Surname,user.Username,user.Password)
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

		public bool Equals(IUserModel other)
		{
			return Equals((UserModel)other);
		}

		protected bool Equals(UserModel other)
		{
			return FirstName == other.FirstName && Surname == other.Surname && Username == other.Username && Password == other.Password;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((UserModel) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(FirstName, Surname, Username, Password);
		}
	}
}