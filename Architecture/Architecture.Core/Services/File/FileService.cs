using System;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public class FileService : BaseService, IFileService
    {
        public FileService(ILocalFileSystemHelper fileSystemHelper)
        {
            this.fileSystemHelper = fileSystemHelper;
        }

        public async Task<bool> DownloadFileAsync(string param, string destinationPath)
        {
            try
            {
                string url = string.Format(ServiceConfig.DOWNLOAD_FILE, param);

                byte[] file = await GetFile(new Uri(url));

                if (!fileSystemHelper.Exists(destinationPath))
                {
                    fileSystemHelper.CreateFile(destinationPath);
                }

                fileSystemHelper.SaveFile(file, destinationPath);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ILocalFileSystemHelper fileSystemHelper;
    }
}
