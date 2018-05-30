using System.Threading.Tasks;

namespace Architecture.Core
{
    public interface IFileService
    {
        Task<bool> DownloadFileAsync(string param, string destinationPath);
    }
}
