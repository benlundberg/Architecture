using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public class SecureRepository : IRepository
    {
        static Lazy<IRepository> implementation;

        public static IRepository Current
        {
            get
            {
                var ret = implementation?.Value;

                if (ret == null)
                {
                    implementation = new Lazy<IRepository>(() => Create(), isThreadSafe: true);
                    ret = implementation.Value;
                }

                return ret;
            }
        }

        private static IRepository Create()
        {
            BlobCache.ApplicationName = AppConfig.AppName;
            return new SecureRepository();
        }

        public void Init(IRepository repository)
        {
            implementation = new Lazy<IRepository>(() => repository, isThreadSafe: true);
        }

        public async Task SaveAsync<T>(string id, T model, TimeSpan expiration = default)
        {
            try
            {
                if (expiration == default)
                {
                    await BlobCache.Secure.InsertObject(id + model.GetType().ToString(), model);
                }
                else
                {
                    await BlobCache.Secure.InsertObject(id + model.GetType().ToString(), model, expiration);
                }
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
                return await BlobCache.Secure.GetObject<T>(id + typeof(T).ToString());
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default;
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

            return default;
        }

        public async Task DeleteAsync<T>(string id)
        {
            try
            {
                await BlobCache.Secure.InvalidateObject<T>(id + typeof(T).ToString());
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
                await BlobCache.Secure.InvalidateAllObjects<T>();
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
                return await BlobCache.Secure.GetAllObjects<T>();
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default;
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

            return default;
        }
    }
}
