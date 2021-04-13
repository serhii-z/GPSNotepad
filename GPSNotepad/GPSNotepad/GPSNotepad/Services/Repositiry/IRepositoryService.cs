using GPSNotepad.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GPSNotepad.Services.Repositiry
{
    public interface IRepositoryService
    {
        Task<int> InsertAsync<T>(T item) where T : IEntityBase, new();
        Task<int> UpdateAsync<T>(T item) where T : IEntityBase, new();
        Task<int> DeleteAsync<T>(T item) where T : IEntityBase, new();
        Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new();
    }
}
