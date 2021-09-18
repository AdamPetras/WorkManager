using System;
using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Enums;
using WorkManager.Models;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class WorkRecordMapper : IMapper<WorkRecordEntity, IWorkRecordModelBase>
	{
		private readonly IMapper<CompanyEntity, ICompanyModel> _companyMapper;
		private readonly IWorkRecordModelFactory _workRecordModelFactory;

		public WorkRecordMapper(IMapper<CompanyEntity,ICompanyModel> companyMapper, IWorkRecordModelFactory workRecordModelFactory)
		{
			_companyMapper = companyMapper;
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
				IdCompany = model.Company.Id,
				Company = _companyMapper.Map(model.Company)
			};
		}

		public IWorkRecordModelBase Map(WorkRecordEntity entity)
		{
			if (entity == null)
				return new WorkBothRecordModel();
			return _workRecordModelFactory.CreateWorkRecord(entity.Id, entity.ActualDateTime, entity.WorkTime,
				entity.PricePerHour, entity.Pieces, entity.PricePerPiece, entity.Type, entity.Description,
				_companyMapper.Map(entity.Company));
		}
	}
}