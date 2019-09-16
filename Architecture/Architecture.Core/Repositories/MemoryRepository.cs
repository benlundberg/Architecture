using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public class MemoryRepository
    {
        static Lazy<MemoryRepository> implementation = new Lazy<MemoryRepository>(() => CreateMemory(), isThreadSafe: true);

        public static MemoryRepository Current
        {
            get
            {
                var ret = implementation.Value;

                if (ret == null)
                {
                    throw new NotImplementedException();
                }

                return ret;
            }
        }

        private static MemoryRepository CreateMemory()
        {
            BlobCache.ApplicationName = AppConfig.AppName;
            return new MemoryRepository();
        }

        public async Task SaveAsync<T>(string id, T model)
        {
            try
            {
                await BlobCache.InMemory.InsertObject(id + model.GetType().ToString(), model);
            }
            catch (Exception ex)
            {
                ex.Print();
            }
        }

        public async Task<T> LoadAsync<T>(string id)
        {
            try
            {
                return await BlobCache.InMemory.GetObject<T>(id + typeof(T).GetType().ToString());
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default(T);
        }

        public async Task<T> LoadAsync<T>(Func<T, bool> predExpr)
        {
            try
            {
                var list = await GetAllAsync<T>();

                return list.FirstOrDefault(predExpr);
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default(T);
        }

        public async Task DeleteAsync<T>(string id)
        {
            try
            {
                await BlobCache.InMemory.InvalidateObject<T>(id + typeof(T).GetType().ToString());
            }
            catch (Exception ex)
            {
                ex.Print();
            }
        }

        public async Task DeleteAllAsync<T>()
        {
            try
            {
                await BlobCache.InMemory.InvalidateAllObjects<T>();
            }
            catch (Exception ex)
            {
                ex.Print();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            try
            {
                return await BlobCache.InMemory.GetAllObjects<T>();
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default(IEnumerable<T>);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Func<T, bool> predExpr)
        {
            try
            {
                var list = await GetAllAsync<T>();

                return list?.Where(predExpr);
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default(IEnumerable<T>);
        }
    }
}
