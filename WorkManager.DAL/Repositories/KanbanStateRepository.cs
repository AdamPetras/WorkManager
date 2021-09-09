﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.BaseClasses;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories
{
	public class KanbanStateRepository:RepositoryBase<KanbanStateEntity>, IKanbanStateRepository
	{
		public KanbanStateRepository(DbContext.Interfaces.IDbContextFactory<WorkManagerDbContext> dbContextFactory) : base(dbContextFactory)
		{
		}

		protected override IEnumerable<KanbanStateEntity> GetAllInt(IQueryable<KanbanStateEntity> dbSet)
		{
			return dbSet.ToList();
		}

		protected override void AddInt(KanbanStateEntity entity, WorkManagerDbContext dbContext)
		{
		}
	}
}