﻿using System;
using System.Collections.Generic;
using WorkManager.DAL.Enums;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface IWorkRecordFacade
	{
		ICollection<IWorkRecordModelBase> GetAllRecordsByCompany(Guid companyId, EFilterType filterType);
	}
}