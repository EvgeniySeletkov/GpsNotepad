using GpsNotepad.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Repository
{
    interface IRepository
    {
        Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new();

        Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new();

        Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new();

        Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new();

    }
}
