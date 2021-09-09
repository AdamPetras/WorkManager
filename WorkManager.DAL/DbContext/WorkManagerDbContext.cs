using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.DAL.Entities.Interfaces;
using Xamarin.Forms;

namespace WorkManager.DAL.DbContext
{
	public class WorkManagerDbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
	{
		private const string DatabaseName = "WorkManager.db";
		public string DatabasePath { get; private set; }
		public DbSet<UserEntity> UserSet { get; set; }
		public DbSet<WorkRecordEntity> WorkSet { get; set; }
		public DbSet<CompanyEntity> CompanySet { get; set; }
		public DbSet<TaskGroupEntity> TaskGroupSet { get; set; }
		public DbSet<TaskEntity> TaskSet { get; set; }
		public DbSet<KanbanStateEntity> KanbanSet { get; set; }
		public DbSet<KanbanTaskGroupEntity> KanbanTaskGroupSet { get; set; }

		public WorkManagerDbContext()
        {

        }

        public WorkManagerDbContext(string databasePath)
        {
	        if (!Directory.Exists(databasePath))
		        Directory.CreateDirectory(databasePath);
	        DatabasePath = databasePath;
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
			if (typeof(T) == typeof(KanbanTaskGroupEntity))
			{
				if (KanbanTaskGroupSet != null)
					return KanbanTaskGroupSet as DbSet<T>;
			}
			throw new ArgumentException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			//modelBuilder.Entity<UserEntity>().HasMany(s => s.Companies).WithOne(s=>s.User).OnDelete(DeleteBehavior.Cascade);
			//modelBuilder.Entity<UserEntity>().HasMany(s => s.TaskGroups).WithOne(s=>s.User).OnDelete(DeleteBehavior.Cascade);
			//modelBuilder.Entity<CompanyEntity>().HasMany(s => s.Items).WithOne(s=>s.Company).OnDelete(DeleteBehavior.Cascade);
			//modelBuilder.Entity<TaskGroupEntity>().HasMany(s => s.TaskCollection).WithOne(s=>s.TaskGroup).OnDelete(DeleteBehavior.Cascade);
			//modelBuilder.Entity<KanbanStateEntity>().HasOne(s => s.Task).WithOne(s=>s.State).HasForeignKey<TaskEntity>(s=>s.IdState);
			//modelBuilder.Entity<KanbanTaskGroupEntity>().HasOne(s => s.Kanban).WithMany(s => s.KanbanTaskGroup).HasForeignKey(s => s.IdKanban);
			//modelBuilder.Entity<KanbanTaskGroupEntity>().HasOne(s => s.TaskGroup).WithMany(s => s.KanbanTaskGroup).HasForeignKey(s => s.IdTaskGroup);
        }
		 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            switch (Device.RuntimePlatform)
            {
	            case Device.iOS:
		            SQLitePCL.Batteries_V2.Init();
		            DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DatabaseName); ;
		            break;
	            case Device.Android:
		            DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName);
		            break;
	            default:
		            throw new NotImplementedException("Platform not supported");
            }
			//File.Delete(DatabasePath);
			optionsBuilder.UseSqlite("Filename=" + DatabasePath);
        }
    }
}