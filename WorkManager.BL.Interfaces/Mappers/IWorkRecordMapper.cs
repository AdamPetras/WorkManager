using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Mappers
{
    public interface IWorkRecordMapper : IMapper<WorkRecordEntity, IWorkRecordModelBase>
    {
        IWorkRecordModelBase Map(WorkRecordEntity entity);
    }
}