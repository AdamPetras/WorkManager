using System;

namespace WorkManager.Models.Interfaces
{
	public interface IWorkPiecesRecordModel : IWorkRecordModelBase, IEquatable<IWorkPiecesRecordModel>
	{
		uint Pieces { get; set; }
		double PricePerPiece { get; set; }
	}
}