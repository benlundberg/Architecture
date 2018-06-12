using Android.Content;
using Android.Webkit;
using Android.Widget;
using Architecture.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Architecture.Droid
{
    public class LocalFileSystemHelper_Droid : ILocalFileSystemHelper
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
                fileStream.Flush();
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

        public void OpenFile(string path)
        {
            try
            {
                string extension = MimeTypeMap.GetFileExtensionFromUrl(path);
                string mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension.ToLower());

                var filename = Path.GetFileName(path);
                var externalPath = Android.OS.Environment.ExternalStorageDirectory.Path + "/" + filename;
                var data = ReadFile(path);
                File.WriteAllBytes(externalPath, data);

                Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(externalPath));

                Intent intent = new Intent()
                    .SetAction(Intent.ActionView)
                    .SetDataAndType(uri, mimeType)
                    .SetFlags(ActivityFlags.NewTask);

                Android.App.Application.Context.StartActivity(Intent.CreateChooser(intent, ""));
            }
            catch (Exception ex)
            {
                ex.Print();
                Toast.MakeText(Android.App.Application.Context, "", ToastLength.Long).Show();
            }
        }
    }
}