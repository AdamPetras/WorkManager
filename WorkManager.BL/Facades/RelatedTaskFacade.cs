using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
    public class RelatedTaskFacade : FacadeBase<IRelatedTaskModel, RelatedTaskEntity>, IRelatedTaskFacade
    {
        protected new readonly IRelatedTaskMapper Mapper;

        public RelatedTaskFacade(WorkManagerDbContext dbContext, IRelatedTaskMapper mapper, IDatabaseSessionController databaseSessionController) : base(dbContext, mapper, databaseSessionController)
        {
            Mapper = mapper;
        }

        public async Task<IRelatedTaskModel> GetByIdAsync(Guid relatedTaskId, CancellationToken token)
        {
            DatabaseSessionController.Reset();
            return Mapper.Map(await DbContext.RelatedTaskSet.SingleOrDefaultAsync(s => s.Id == relatedTaskId, token));
        }
    }
}