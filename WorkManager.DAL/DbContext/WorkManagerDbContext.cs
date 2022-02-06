using System;
using Microsoft.EntityFrameworkCore;
using PCLAppConfig;
using WorkManager.DAL.DbContext.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.DbContext
{
	public class WorkManagerDbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
	{
		public DbSet<UserEntity> UserSet { get; set; }
		public DbSet<WorkRecordEntity> WorkSet { get; set; }
		public DbSet<CompanyEntity> CompanySet { get; set; }
		public DbSet<TaskGroupEntity> TaskGroupSet { get; set; }
		public DbSet<TaskEntity> TaskSet { get; set; }
		public DbSet<KanbanStateEntity> KanbanSet { get; set; }
		public DbSet<ImageEntity> ImageSet { get; set; }

		public WorkManagerDbContext()
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActualDateTimeEntity>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(nameof(ActualDateTimeEntity));
            });
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                string connStr = ConfigurationManager.AppSettings["connectionstring"];	//při migraci je potřeba vykopírovat
                optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
			}
		}
    }
}