using System;
using System.Collections.Generic;
using System.Linq;
using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class CompanyMapper: IMapper<CompanyEntity, ICompanyModel>
	{
		private readonly IMapper<UserEntity, IUserModel> _userMapper;

		public CompanyMapper(IMapper<UserEntity, IUserModel> userMapper)
		{
			_userMapper = userMapper;
		}

		public CompanyEntity Map(ICompanyModel model)
		{
			if (model == null)
				return new CompanyEntity();
			return new CompanyEntity()
			{
				Id = model.Id,
				Name = model.Name,
				IdUser = model.User.Id,
				User = _userMapper.Map(model.User)
			};
		}

		public ICompanyModel Map(CompanyEntity entity)
		{
			if (entity == null)
				return new CompanyModel();
			return new CompanyModel(entity.Id,entity.Name, _userMapper.Map(entity.User));
		}
	}
}