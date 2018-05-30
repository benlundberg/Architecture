using System;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public class LoginService : BaseService, ILoginService
    {
        public async Task<T> LoginAsync<T>(string username, string password)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return default(T);
        }
    }
}
