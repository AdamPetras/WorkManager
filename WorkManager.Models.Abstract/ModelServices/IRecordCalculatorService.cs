namespace WorkManager.Models.Interfaces.ModelServices
{
	public interface IRecordCalculatorService
	{
		double Calculate<T>(T record) where T : IWorkRecordModelBase;
	}
}