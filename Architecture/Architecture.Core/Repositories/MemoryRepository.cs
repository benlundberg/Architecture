using Akavache;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public class MemoryKey
    {

    }

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

        public async Task SaveAsync<T>(string key, T model)
        {
            try
            {
                await BlobCache.InMemory.InsertObject<T>(key, model);
            }
            catch (Exception ex)
            {
                ex.Print();
            }
        }

        public async Task<T> LoadAsync<T>(string key)
        {
            try
            {
                return await BlobCache.InMemory.GetObject<T>(key);
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default(T);
        }

        public async Task DeleteAsync<T>(string key)
        {
            try
            {
                await BlobCache.InMemory.InvalidateObject<T>(key);
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
    }
}
