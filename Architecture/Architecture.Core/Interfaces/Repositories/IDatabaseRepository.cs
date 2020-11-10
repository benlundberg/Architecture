using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public interface IDatabaseRepository
    {
        void Init(IDatabaseRepository database);
        void Init(string databasePath);
        Task<bool> ClearAsync<T>() where T : new();
        Task<bool> DeleteAsync<T>(List<T> list);
        Task<bool> DeleteAsync<T>(T entity);
        Task<IEnumerable<T>> LoadAllAsync<T>() where T : new();
        Task<IEnumerable<T>> LoadAllAsync<T>(string query) where T : new();
        Task<IEnumerable<T>> LoadAllAsync<T>(Expression<Func<T, bool>> predExpr) where T : new();
        Task<T> LoadAsync<T>(object id) where T : new();
        Task<T> LoadAsync<T>(string query) where T : new();
        Task<T> LoadWithChildrenAsync<T>(object id) where T : new();
        Task<bool> InsertAsync<T>(List<T> entities);
        Task<bool> InsertAsync<T>(T entity);
        Task<bool> InsertOrReplaceWithChildrenAsync<T>(T entity);
        Task<bool> InsertOrReplaceAsync<T>(T entity);
        Task<bool> UpdateAsync<T>(T entity);
        Task ExecuteQueryAsync(string query);
        Task<bool> CreateTablesAsync(List<Type> entities);
    }
}
