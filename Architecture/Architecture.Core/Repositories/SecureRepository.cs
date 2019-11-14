using Akavache;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public class SecureRepository
    {
        static readonly Lazy<SecureRepository> implementation = new Lazy<SecureRepository>(() => CreateSecure(), isThreadSafe: true);

        public static SecureRepository Current
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

        private static SecureRepository CreateSecure()
        {
            BlobCache.ApplicationName = AppConfig.AppName;
            return new SecureRepository();
        }

        public async Task SaveAsync<T>(string id, T model)
        {
            try
            {
                await BlobCache.Secure.InsertObject(id + model.GetType().ToString(), model);
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
    }
}
