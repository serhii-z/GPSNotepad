using GPSNotepad.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Repositiry
{
    public class RepositoryService : IRepositoryService
    {
        private Lazy<SQLiteAsyncConnection> _database;
        public RepositoryService()
        {
            _database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.DatabaseName);
                var database = new SQLiteAsyncConnection(path);

                database.CreateTableAsync<UserModel>().Wait();
                database.CreateTableAsync<PinModel>().Wait();

                return database;
            });
        }

        #region -- IRepositoryService implement --

        public Task<int> InsertAsync<T>(T item) where T : IEntityBase, new()
        {
            return _database.Value.InsertAsync(item);
        }

        public Task<int> UpdateAsync<T>(T item) where T : IEntityBase, new()
        {
            return _database.Value.UpdateAsync(item);
        }

        public Task<int> DeleteAsync<T>(T item) where T : IEntityBase, new()
        {
            return _database.Value.DeleteAsync(item);
        }

        public Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new()
        {
            return _database.Value.Table<T>().ToListAsync();
        }

        #endregion
    }
}
