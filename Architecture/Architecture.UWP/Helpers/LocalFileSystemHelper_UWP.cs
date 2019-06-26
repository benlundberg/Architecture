using Architecture.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Architecture.UWP.Helpers
{
    class LocalFileSystemHelper_UWP : ILocalFileSystemHelper
    {
        public string LocalStorage => Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public string GetLocalPath(params string[] paths)
        {
            List<string> pathList = paths.ToList();
            pathList.Insert(0, LocalStorage);

            return Path.Combine(pathList.ToArray());
        }

        public string CreateFile(params string[] paths)
        {
            var path = GetLocalPath(paths);

            if (!File.Exists(path))
            {
                var fileStream = File.Create(path);

                fileStream.Close();
            }

            return path;
        }

        public string CreateFolder(params string[] paths)
        {
            string path = GetLocalPath(paths);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public FileStream GetFileStream(params string[] paths)
        {
            var path = GetLocalPath(paths);

            if (!File.Exists(path))
            {
                path = CreateFile(paths);
            }

            return File.Create(path);
        }

        public IEnumerable<string> GetFiles(params string[] paths)
        {
            var path = GetLocalPath(paths);

            if (Directory.Exists(path))
            {
                return Directory.EnumerateFiles(path);
            }

            return new List<string>();
        }

        public bool Delete(params string[] paths)
        {
            var path = GetLocalPath(paths);

            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return true;
        }

        public bool Exists(params string[] paths)
        {
            var path = GetLocalPath(paths);

            if (Directory.Exists(path))
            {
                return true;
            }

            if (File.Exists(path))
            {
                return true;
            }

            return false;
        }

        public bool Move(string sourcePath, string destinationPath)
        {
            if (Directory.Exists(sourcePath))
            {
                Directory.Move(sourcePath, destinationPath);
                return true;
            }
            else if (File.Exists(sourcePath))
            {
                File.Move(sourcePath, destinationPath);
                return true;
            }

            return false;
        }

        public byte[] ReadFile(params string[] paths)
        {
            string path = GetLocalPath(paths);

            try
            {
                return File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return null;
        }

        public string SaveFile(byte[] data, params string[] paths)
        {
            string path = GetLocalPath(paths);

            if (!Exists(path))
            {
                path = CreateFile(paths);
            }

            File.WriteAllBytes(path, data);

            return path;
        }

        public string WriteText(string text, bool append, params string[] paths)
        {
            string path = GetLocalPath(paths);

            if (!Exists(path))
            {
                path = CreateFile(paths);
            }

            if (append)
            {
                File.AppendAllText(path, text);
            }
            else
            {
                File.WriteAllText(path, text);
            }

            return path;
        }

        public string ReadText(params string[] paths)
        {
            string path = GetLocalPath(paths);

            if (!Exists(path))
            {
                path = CreateFile(paths);
            }

            return File.ReadAllText(path);
        }
    }
}
