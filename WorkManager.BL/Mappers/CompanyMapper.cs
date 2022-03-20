using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class CompanyMapper: ICompanyMapper
	{

        public CompanyMapper()
        {
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

        public ICompanyModel Map(CompanyEntity entity, int workRecordsCount)
        {
            if (entity == null)
                return new CompanyModel();
            return new CompanyModel(entity.Id, entity.Name, workRecordsCount, entity.UserId);
        }
	}
}