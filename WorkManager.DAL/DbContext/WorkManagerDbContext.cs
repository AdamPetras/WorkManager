using System;
using System.IO;
using System.Reflection;
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
		private readonly DatabaseVersionChecker _databaseVersionChecker;
		private const string DatabaseName = "WorkManager.db";
		public string DatabasePath { get; private set; }
		public DbSet<UserEntity> UserSet { get; set; }
		public DbSet<WorkRecordEntity> WorkSet { get; set; }
		public DbSet<CompanyEntity> CompanySet { get; set; }
		public DbSet<TaskGroupEntity> TaskGroupSet { get; set; }
		public DbSet<TaskEntity> TaskSet { get; set; }
		public DbSet<KanbanStateEntity> KanbanSet { get; set; }
		public DbSet<ImageEntity> ImageSet { get; set; }

		public WorkManagerDbContext()
		{
			_databaseVersionChecker = new DatabaseVersionChecker();
		}

        public WorkManagerDbContext(string databasePath)
        {
			_databaseVersionChecker = new DatabaseVersionChecker();
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

			if (File.Exists(DatabasePath))
			{
				if (!_databaseVersionChecker.IsVersionTableInsideDatabase(DatabasePath))
				{
					File.Delete(DatabasePath);  //pokud není tabulka verzí v databázi nemělo by se stávat vůbec je to zatím pro ty co měly nainstalováno jako interní testy
				}
				else if (!_databaseVersionChecker.IsDatabaseCorrectVersion(DatabasePath, Assembly.GetExecutingAssembly().GetName().Version.ToString()))
				{
					//tady by měla být implementována změna z verze xx na yy zatím není nasazeno tak nebudu dělat
					File.Delete(DatabasePath);  //pokud není správná verze databáze
				}
			}
			optionsBuilder.UseSqlite("Filename=" + DatabasePath);
        }
    }
}