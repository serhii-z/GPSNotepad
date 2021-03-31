using GPSNotepad.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Repositiry
{
    public class Repository : IRepository
    {
        private Lazy<SQLiteAsyncConnection> _database;
        public Repository()
        {
            _database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "profilebook.db");
                var database = new SQLiteAsyncConnection(path);

                database.CreateTableAsync<UserModel>();
                database.CreateTableAsync<PinModel>();

                return database;
            });
        }

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
    }
}
