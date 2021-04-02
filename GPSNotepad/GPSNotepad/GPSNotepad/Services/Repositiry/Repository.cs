using GPSNotepad.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Repositiry
{
    public class Repository
    {
        private static Lazy<SQLiteAsyncConnection> database;
        public Repository()
        {
            database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "profilebook.db");
                var database = new SQLiteAsyncConnection(path);

                database.CreateTableAsync<UserModel>();
                database.CreateTableAsync<PinModel>();

                return database;
            });
        }

        public static Task<int> InsertAsync<T>(T item) where T : IEntityBase, new()
        {
            return database.Value.InsertAsync(item);
        }

        public static Task<int> UpdateAsync<T>(T item) where T : IEntityBase, new()
        {
            return database.Value.UpdateAsync(item);
        }

        public static Task<int> DeleteAsync<T>(T item) where T : IEntityBase, new()
        {
            return database.Value.DeleteAsync(item);
        }

        public static Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new()
        {
            return database.Value.Table<T>().ToListAsync();
        }
    }
}
