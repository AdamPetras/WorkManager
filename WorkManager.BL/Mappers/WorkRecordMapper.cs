using System;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class WorkRecordMapper : IWorkRecordMapper
	{
		private readonly IWorkRecordModelFactory _workRecordModelFactory;

		public WorkRecordMapper(IWorkRecordModelFactory workRecordModelFactory)
		{
			_workRecordModelFactory = workRecordModelFactory;
		}

		public WorkRecordEntity Map(IWorkRecordModelBase model)
		{
			if (model == null)
				return new WorkRecordEntity();
			uint pieces = 0;
			double pricePerPiece = 0;
			TimeSpan workTime = TimeSpan.Zero;
			double pricePerHour = 0;
			switch (model)
			{
				case IWorkBothRecordModel bothRecordModel:
					pieces = bothRecordModel.Pieces;
					pricePerPiece = bothRecordModel.PricePerPiece;
					workTime = bothRecordModel.WorkTime;
					pricePerHour = bothRecordModel.PricePerHour;
					break;
				case IWorkPiecesRecordModel piecesRecordModel:
					pieces = piecesRecordModel.Pieces;
					pricePerPiece = piecesRecordModel.PricePerPiece;
					break;
				case IWorkTimeRecordModel timeRecordModel:
					workTime = timeRecordModel.WorkTime;
					pricePerHour = timeRecordModel.PricePerHour;
					break;
				default:
					throw new ArgumentException();
			}

			return new WorkRecordEntity()
			{
				Id = model.Id,
				ActualDateTime = model.ActualDateTime,
				Type = model.Type,
				Pieces = pieces,
				PricePerPiece = pricePerPiece,
				WorkTime = workTime,
				PricePerHour = pricePerHour,
				Description = model.Description,
				CompanyId = model.CompanyId,
			};
		}

		public IWorkRecordModelBase Map(WorkRecordEntity entity)
		{
			if (entity == null)
				return new WorkBothRecordModel();
			return _workRecordModelFactory.CreateWorkRecord(entity.Id, entity.ActualDateTime, entity.WorkTime,
				entity.PricePerHour, entity.Pieces, entity.PricePerPiece, entity.Type, entity.Description, entity.CompanyId);
		}
    }
}