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

		public IEnumerable<IImageModel> GetAllImagesByTask(Guid id)
		{
			return Repository.GetAllImagesByTask(id).Select(Mapper.Map);
		}

        public async IAsyncEnumerable<IImageModel> GetAllImagesByTaskAsync(Guid id, CancellationToken token = default)
        {
            foreach (ImageEntity value in await Repository.GetAllImagesByTaskAsync(id, token))
            {
                yield return await Mapper.MapAsync(value, token);
            }
        }
    }
}