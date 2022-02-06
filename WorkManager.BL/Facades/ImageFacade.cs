using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models.Interfaces;
using Xamarin.Forms;

namespace WorkManager.BL.Facades
{
	public class ImageFacade : FacadeBase<IImageModel, ImageEntity>, IImageFacade
	{
		public new IImageRepository Repository;
		public ImageFacade(IImageRepository repository, IMapper<ImageEntity, IImageModel> mapper) : base(repository, mapper)
		{
			Repository = repository;
		}

		public ICollection<IImageModel> GetAllImagesByTask(Guid id)
		{
			return Repository.GetWhere(s=> s.TaskId == id).Select(Mapper.Map).ToList();
		}

        public async Task<ICollection<IImageModel>> GetAllImagesByTaskAsync(Guid id, CancellationToken token = default)
        {
			return (await Repository.GetWhereAsync(s=> s.TaskId == id, token).ConfigureAwait(false)).Select(Mapper.Map).ToList();
        }
    }
}