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
        public RelatedTaskFacade(WorkManagerDbContext dbContext, IMapper<RelatedTaskEntity, IRelatedTaskModel> mapper, IDatabaseSessionController databaseSessionController) : base(dbContext, mapper, databaseSessionController)
        {
        }
    }
}