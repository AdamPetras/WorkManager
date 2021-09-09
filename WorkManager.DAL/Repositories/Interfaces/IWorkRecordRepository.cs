using System;
using System.Collections.Generic;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Enums;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface IWorkRecordRepository : IRepository<WorkRecordEntity>
	{
		IEnumerable<WorkRecordEntity> GetAllRecordsByCompany(Guid companyId, EFilterType filterType);
	}
}