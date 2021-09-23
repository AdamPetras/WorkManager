using System;
using System.Linq;
using System.Reflection;
using SQLite;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.DbContext.Interfaces
{
	public class DatabaseVersionChecker
	{
		public bool IsVersionTableInsideDatabase(string databasePath)
		{
			using (SQLiteConnection connection = new SQLiteConnection(new SQLiteConnectionString(databasePath)))
			{
				SQLiteCommand cmd = connection.CreateCommand("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'VersionEntity';");
				return cmd.ExecuteScalar<string>() != null;
			}
		}

		public void CreateTableVersionIfNotExists(string databasePath)
		{
			using (SQLiteConnection connection = new SQLiteConnection(new SQLiteConnectionString(databasePath)))
			{
				SQLiteCommand cmd =
					connection.CreateCommand(
						"CREATE TABLE IF NOT EXISTS VersionEntity (Id TEXT PRIMARY KEY, Version TEXT);");
				cmd.ExecuteNonQuery();
				connection.Commit();
			}
		}

		public bool IsDatabaseCorrectVersion(string databasePath, string version)
		{
			using (SQLiteConnection connection = new SQLiteConnection(new SQLiteConnectionString(databasePath)))
			{
				VersionEntity entity = connection.Table<VersionEntity>().Last();
				return entity.Version == version;
			}
		}

		public void WriteLatestVersion(string databasePath)
		{
			using (SQLiteConnection connection = new SQLiteConnection(new SQLiteConnectionString(databasePath)))
			{
				if (connection.Table<VersionEntity>().Any())
					connection.DeleteAll<VersionEntity>();
				connection.Insert(new VersionEntity()
					{Id = Guid.NewGuid(), Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()});
			}
		}
	}
}