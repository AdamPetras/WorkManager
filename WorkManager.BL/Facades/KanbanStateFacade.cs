using System;
using System.Collections.Generic;
using System.Linq;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class KanbanStateFacade:FacadeBase<IKanbanStateModel,KanbanStateEntity>, IKanbanStateFacade
	{
		protected new IKanbanStateRepository Repository;
		public KanbanStateFacade(IKanbanStateRepository repository, IMapper<KanbanStateEntity, IKanbanStateModel> mapper) : base(repository, mapper)
		{
			Repository = repository;
			InitializeDefaultValues();
		}

		private void InitializeDefaultValues()
		{
			foreach (IKanbanStateModel model in GetDefaultKanbanStateModels())
			{
				if(!ModelExists(model.Id))
					Add(model);
			}
		}

		public IEnumerable<IKanbanStateModel> GetDefaultKanbanStateModels()
		{
			yield return new KanbanStateModel(new Guid("CFC911E6-3BD5-4508-9B25-73801A3D799F"), "Todo", 0, "\uf46d");
			yield return new KanbanStateModel(new Guid("0150EACA-302A-4FDC-9CEC-920C9152AD03"), "In Progress", 1, "\uf110");
			yield return new KanbanStateModel(new Guid("9C5D2C07-90C2-4BDD-9DAD-81F92E7072AA"), "Done", 2, "\uf46c");
		}

		public IEnumerable<IKanbanStateModel> GetKanbanStateByTaskGroup(Guid taskGroupId)
		{
			return Repository.GetKanbanStateByTaskGroup(taskGroupId).Select(Mapper.Map);
		}
	}
}