namespace WorkManager.Models.Interfaces
{
	public interface IUserModel : IModel
	{
		string FirstName { get; set; }
		string Surname { get; set; }
		string Username { get; set; }
		string Password { get; set; }
	}
}