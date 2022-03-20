using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class ImageFacade : FacadeBase<IImageModel, ImageEntity>, IImageFacade
	{
        protected new readonly IImageMapper Mapper;

        public ImageFacade(WorkManagerDbContext dbContext, IImageMapper mapper,
            IDatabaseSessionController databaseSessionController) : base(dbContext, mapper, databaseSessionController)
        {
            Mapper = mapper;
        }

		public ICollection<IImageModel> GetAllImagesByTask(Guid id)
		{
            DatabaseSessionController.Reset();
			return DbContext.ImageSet.AsQueryable().Where(s=> s.TaskId == id).ToList().Select(Mapper.Map).ToList();
		}

        public async Task<ICollection<IImageModel>> GetAllImagesByTaskAsync(Guid id, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
			return await (await DbContext.ImageSet.AsQueryable().Where(s=> s.TaskId == id).ToListAsync(token)).Select(Mapper.Map).ToAsyncEnumerable().ToListAsync(token);
        }
    }
}