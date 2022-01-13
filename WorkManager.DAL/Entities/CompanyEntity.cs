﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.DAL.Entities.Interfaces;
using WorkManager.DAL.Enums;

namespace WorkManager.DAL.Entities
{
	public class CompanyEntity : IEntity
	{
		public CompanyEntity()
		{
		}
        [Key]
        public Guid Id { get; set; }
		public string Name { get; set; }
		public EWorkType Type { get; set; }

		[Required]
		public Guid UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public virtual UserEntity User { get; set; }
	}
}