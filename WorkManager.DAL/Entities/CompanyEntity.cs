using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.DAL.Enums;

namespace WorkManager.DAL.Entities
{
	public class CompanyEntity : EntityBase
	{
		public CompanyEntity()
		{
		}

		public string Name { get; set; }
		public EWorkType Type { get; set; }

		[Required]
		public Guid IdUser { get; set; }

		[ForeignKey(nameof(IdUser))]
		public virtual UserEntity User { get; set; }
	}
}