using System.Threading.Tasks;

namespace Architecture.Core
{
    public interface ILoginService
    {
        Task<T> LoginAsync<T>(string username, string password);
    }
}
