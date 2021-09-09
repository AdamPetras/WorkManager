namespace WorkManager.Models.Interfaces
{
	public interface IWorkPiecesRecordModel : IWorkRecordModelBase
	{
		uint Pieces { get; set; }
		double PricePerPiece { get; set; }
	}
}