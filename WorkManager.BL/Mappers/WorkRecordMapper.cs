using System;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Enums;
using WorkManager.Models;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;
using WorkManager.Xamarin.Core;

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
				Type = model.Type.GetValue<EWorkType>(),
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
				entity.PricePerHour, entity.Pieces, entity.PricePerPiece, new LocalizedEnum(entity.Type), entity.Description, entity.CompanyId);
		}

        public Task<WorkRecordEntity> MapAsync(IWorkRecordModelBase model, CancellationToken token)
        {
			if (model == null)
                return Task.FromResult(new WorkRecordEntity());
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

            return Task.FromResult(new WorkRecordEntity()
            {
                Id = model.Id,
                ActualDateTime = model.ActualDateTime,
                Type = model.Type.GetValue<EWorkType>(),
                Pieces = pieces,
                PricePerPiece = pricePerPiece,
                WorkTime = workTime,
                PricePerHour = pricePerHour,
                Description = model.Description,
                CompanyId = model.CompanyId,
            });
		}

        public Task<IWorkRecordModelBase> MapAsync(WorkRecordEntity entity, CancellationToken token)
        {
			if (entity == null)
                return Task.FromResult<IWorkRecordModelBase>(new WorkBothRecordModel());
            return Task.FromResult(_workRecordModelFactory.CreateWorkRecord(entity.Id, entity.ActualDateTime, entity.WorkTime,
                entity.PricePerHour, entity.Pieces, entity.PricePerPiece, new LocalizedEnum(entity.Type), entity.Description, entity.CompanyId));
		}
    }
}