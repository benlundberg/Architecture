using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public class ZipProvider : IZipProvider
    {
        public ZipProvider(ILocalFileSystemHelper fileSystemHelper)
        {
            this.fileSystemHelper = fileSystemHelper;
        }

        public async Task<bool> UnzipFileAsync(string fromPath, string toPath, Action<decimal> progress, List<string> exceptionFileTypes = null)
        {
            try
            {
                await slimUnzip.WaitAsync();

                if (!fileSystemHelper.Exists(fromPath))
                {
                    throw new Exception("Could not find file");
                }

                ZipFile zipFile = null;

                try
                {

                    using (Stream zipFileStream = File.Open(fromPath, FileMode.Open))
                    {
                        zipFile = new ZipFile(zipFileStream);

                        long fileCount = zipFile.Count;
                        long filesUnzipped = 0;
                        int cnt = 0;

                        if (fileCount == 0)
                        {
                            throw new Exception("File is empty");
                        }

                        foreach (ZipEntry zipEntry in zipFile)
                        {
                            filesUnzipped++;

                            if (string.IsNullOrEmpty(zipEntry.Name) || (!zipEntry.IsFile && (exceptionFileTypes?.Contains(zipEntry.Name) == true)))
                            {
                                continue;
                            }

                            cnt++;

                            if (cnt == 4)
                            {
                                var prog = (int)Math.Round(((float)filesUnzipped / fileCount) * 100, 2);
                                progress?.Invoke(prog);
                                await Task.Yield();
                                cnt = 0;
                            }

                            string fullPath = fileSystemHelper.GetLocalPath(toPath, Path.GetDirectoryName(zipEntry.Name), Path.GetFileName(zipEntry.Name));

                            using (var fileStream = File.Create(fullPath))
                            {
                                byte[] buffer = new byte[4096];

                                using (Stream zipStream = zipFile.GetInputStream(zipEntry))
                                {
                                    StreamUtils.Copy(zipStream, fileStream, buffer);
                                }
                            }
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (zipFile != null)
                    {
                        zipFile.IsStreamOwner = true;
                        zipFile.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                slimUnzip.Release();
            }
        }

        private static SemaphoreSlim slimUnzip = new SemaphoreSlim(1, 1);

        private ILocalFileSystemHelper fileSystemHelper;
    }
}
