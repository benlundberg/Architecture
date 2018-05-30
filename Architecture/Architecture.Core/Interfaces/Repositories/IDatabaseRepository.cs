using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public interface IDatabaseRepository
    {
        Task<IEnumerable<T>> GetAsync<T>() where T : new();
        Task<T> GetAsync<T>(object id) where T : new();
        Task<bool> InsertAsync<T>(T entity);
        Task<bool> InsertAsync<T>(List<T> entities);
        Task<bool> InsertOrReplaceAsync<T>(T entity);
        Task<bool> UpdateAsync<T>(T entity);
        Task<bool> DeleteAsync<T>(T entity);
        Task<bool> DeleteAsync<T>(List<T> list);
        Task<bool> ClearAsync<T>() where T : new();
        Task<bool> CreateTablesAsync(List<Type> entities);
    }
}
