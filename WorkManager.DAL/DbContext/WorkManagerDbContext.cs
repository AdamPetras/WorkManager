using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.DAL.Entities.Interfaces;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using PCLAppConfig;
using Xamarin.Forms;

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

        public DbSet<T> GetDatabaseByType<T>() where T : class, IEntity
        {
			if (typeof(T) == typeof(UserEntity))
			{
				if (UserSet != null)
					return UserSet as DbSet<T>;
			}
			if (typeof(T) == typeof(WorkRecordEntity))
			{
				if (WorkSet != null)
					return WorkSet as DbSet<T>;
			}
			if (typeof(T) == typeof(TaskEntity))
			{
				if (TaskSet != null)
					return TaskSet as DbSet<T>;
			}
			if (typeof(T) == typeof(TaskGroupEntity))
			{
				if (TaskGroupSet != null)
					return TaskGroupSet as DbSet<T>;
			}
			if (typeof(T) == typeof(CompanyEntity))
			{
				if (CompanySet != null)
					return CompanySet as DbSet<T>;
			}
			if (typeof(T) == typeof(KanbanStateEntity))
			{
				if (KanbanSet != null)
					return KanbanSet as DbSet<T>;
			}
			if (typeof(T) == typeof(ImageEntity))
			{
				if (ImageSet != null)
					return ImageSet as DbSet<T>;
			}
			throw new ArgumentException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			
        }
		 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                PostgreSqlConnectionStringBuilder builder = new PostgreSqlConnectionStringBuilder(ConfigurationManager.AppSettings["connectionstring"])
                {
                    Pooling = true,
                    TrustServerCertificate = true,
                    SslMode = SslMode.Require
                };

                optionsBuilder.UseNpgsql(builder.ConnectionString);
			}
        }
    }
}