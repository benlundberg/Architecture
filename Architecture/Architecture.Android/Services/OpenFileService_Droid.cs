using System;
using System.IO;
using Android.Content;
using Android.Support.V4.Content;
using Android.Webkit;
using Architecture.Core;

namespace Architecture.Droid
{
    public class OpenFileService_Droid : IOpenFileService
    {
        public OpenFileService_Droid(ILocalFileSystemService localFileSystemService)
        {
            this.localFileSystemService = localFileSystemService;
        }

        public void Open(string title, params string[] paths)
        {
            // Get path for file
            var path = localFileSystemService.GetLocalPath(paths);

            // Get extension and mimetype
            var extension = MimeTypeMap.GetFileExtensionFromUrl(path);
            var mimetype = MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension.ToLower());
        
            // Get external path
            var externalPath = global::Android.App.Application.Context.GetExternalFilesDir("").Path + "/" + Path.GetFileName(path);

            // Write file bytes to external path
            File.WriteAllBytes(externalPath, localFileSystemService.ReadFile(path));

            var uri = FileProvider.GetUriForFile(global::Android.App.Application.Context, global::Android.App.Application.Context.ApplicationContext.PackageName + ".fileprovider", new Java.IO.File(externalPath));

            var intent = new Intent();
            intent.SetAction(Intent.ActionView);
            intent.SetDataAndType(uri, mimetype);
            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission);

            var chooserIntent = Intent.CreateChooser(intent, title);

            chooserIntent.SetAction(Intent.ActionView);
            chooserIntent.SetDataAndType(uri, mimetype);
            chooserIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission);

            try
            {
                global::Android.App.Application.Context.StartActivity(chooserIntent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private readonly ILocalFileSystemService localFileSystemService;
    }
}