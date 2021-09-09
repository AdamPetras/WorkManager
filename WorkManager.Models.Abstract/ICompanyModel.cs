using WorkManager.DAL.Enums;

namespace WorkManager.Models.Interfaces
{
	public interface ICompanyModel:IModel
	{
		string Name { get; set; }
		IUserModel User { get; set; }
	}
}