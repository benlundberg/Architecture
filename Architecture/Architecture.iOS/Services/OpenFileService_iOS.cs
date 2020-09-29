using System;
using System.IO;
using Architecture.Core;
using Foundation;
using QuickLook;
using UIKit;

namespace Architecture.iOS
{
    public class OpenFileService_iOS : IOpenFileService
    {
        public OpenFileService_iOS(ILocalFileSystemService localFileSystemService)
        {
            this.localFileSystemService = localFileSystemService;
        }

        public void Open(string title, params string[] paths)
        {
            // Get path
            var path = localFileSystemService.GetLocalPath(paths);
            
            // Get filename
            var fileName = Path.GetFileName(path);

            UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (currentController.PresentedViewController != null)
            {
                currentController = currentController.PresentedViewController;
            }

            UIView currentView = currentController.View;

            QLPreviewController qlPreview = new QLPreviewController();

            QLPreviewItem item = new QLPreviewItemBundle(fileName, path);

            qlPreview.DataSource = new PreviewControllerDS(item);

            currentController.PresentViewController(qlPreview, true, null);
        }

        private readonly ILocalFileSystemService localFileSystemService;
    }

    public class QLPreviewItemBundle : QLPreviewItem
    {
        // Setting file name and path
        public QLPreviewItemBundle(string fileName, string filePath)
        {
            this.fileName = fileName;
            this.filePath = filePath;
        }

        // Return file name
        public override string ItemTitle
        {
            get
            {
                return fileName;
            }
        }

        // Retun file path as NSUrl
        public override NSUrl ItemUrl
        {
            get
            {
                var documents = NSBundle.MainBundle.BundlePath;
                var lib = Path.Combine(documents, filePath);
                var url = NSUrl.FromFilename(lib);
                return url;
            }
        }

        private string fileName, filePath;
    }

    public class QLPreviewItemFileSystem : QLPreviewItem
    {
        // Setting file name and path
        public QLPreviewItemFileSystem(string fileName, string filePath)
        {
            this.fileName = fileName;
            this.filePath = filePath;
        }

        // Return file name
        public override string ItemTitle
        {
            get
            {
                return fileName;
            }
        }

        // Retun file path as NSUrl
        public override NSUrl ItemUrl
        {
            get
            {
                return NSUrl.FromFilename(filePath);
            }
        }

        private string fileName, filePath;
    }

    public class PreviewControllerDS : QLPreviewControllerDataSource
    {
        // Setting the document
        public PreviewControllerDS(QLPreviewItem item)
        {
            this.item = item;
        }

        // Setting document count to 1
        public override nint PreviewItemCount(QLPreviewController controller)
        {
            return 1;
        }

        // Return the document
        public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
        {
            return item;
        }
    
        // Document cache
        private QLPreviewItem item;
    }
}