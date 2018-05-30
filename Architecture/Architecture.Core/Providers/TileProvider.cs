using System;
using System.IO;

namespace Architecture.Core
{
    public class TileProvider : ITileProvider
    {
        public TileProvider(ILocalFileSystemHelper fileSystemHelper)
        {
            this.fileSystemHelper = fileSystemHelper;
        }

        public byte[] GetTile(string path, int x, int y, int zoom, string extension = ".png")
        {
            byte[] data = default(byte[]);

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    y = (int)(Math.Pow(2, zoom) - y - 1);

                    string filePath = fileSystemHelper.GetLocalPath(path, zoom.ToString(), x.ToString(), y.ToString() + extension);

                    data = fileSystemHelper.ReadFile(filePath);
                }
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return data;
        }

        private ILocalFileSystemHelper fileSystemHelper;
    }
}
