using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Mappers
{
    public interface ICompanyMapper : IMapper<CompanyEntity, ICompanyModel>
    {
        ICompanyModel Map(CompanyEntity entity, int workRecordsCount);
    }
}