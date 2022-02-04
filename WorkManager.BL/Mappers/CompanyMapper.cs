using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class CompanyMapper: IMapper<CompanyEntity, ICompanyModel>
	{
		private readonly IMapper<UserEntity, IUserModel> _userMapper;
        private readonly IWorkRecordRepository _workRecordRepository;

        public CompanyMapper(IMapper<UserEntity, IUserModel> userMapper, IWorkRecordRepository workRecordRepository)
        {
            _userMapper = userMapper;
            _workRecordRepository = workRecordRepository;
        }

		public CompanyEntity Map(ICompanyModel model)
		{
			if (model == null)
				return new CompanyEntity();
			return new CompanyEntity()
			{
				Id = model.Id,
				Name = model.Name,
				UserId = model.UserId,
			};
		}

		public ICompanyModel Map(CompanyEntity entity)
		{
			if (entity == null)
				return new CompanyModel();
			return new CompanyModel(entity.Id,entity.Name, _workRecordRepository.Count(s=>s.CompanyId == entity.Id), entity.UserId);
		}

        public Task<CompanyEntity> MapAsync(ICompanyModel model, CancellationToken token)
        {
            if (model == null)
                return Task.FromResult(new CompanyEntity());
            return Task.FromResult(new CompanyEntity()
            {
                Id = model.Id,
                Name = model.Name,
                UserId = model.UserId,
            });
        }

        public async Task<ICompanyModel> MapAsync(CompanyEntity entity, CancellationToken token)
        {
            if (entity == null)
                return new CompanyModel();
            return new CompanyModel(entity.Id, entity.Name, await _workRecordRepository.CountAsync(s=>s.CompanyId == entity.Id, token), entity.UserId);
        }
	}
}