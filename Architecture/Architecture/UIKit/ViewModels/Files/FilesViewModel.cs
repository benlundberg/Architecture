using Architecture.Core;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Architecture
{
    public class FilesViewModel : BaseService
    {
        public FilesViewModel()
        {
            this.localFileSystemService = ComponentContainer.Current.Resolve<ILocalFileSystemService>();
        }

        private ICommand openPdfCommand;
        public ICommand OpenPdfCommand => openPdfCommand ?? (openPdfCommand = new Command(async () =>
        {
            try
            {
                string url = "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf";

                string path = await DownloadFileAsync(url, "Test", ".pdf");

                ComponentContainer.Current.Resolve<IOpenFileService>().Open("", path);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "Ok");
            }
        }));

        private ICommand openLauncherPdfCommand;
        public ICommand OpenLauncherPdfCommand => openLauncherPdfCommand ?? (openLauncherPdfCommand = new Command(async () =>
        {
            try
            {
                string url = "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf";

                string path = await DownloadFileAsync(url, "Test", ".pdf");

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(path)
                });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "Ok");
            }
        }));

        private ICommand openLauncherWordCommand;
        public ICommand OpenLauncherWordCommand => openLauncherWordCommand ?? (openLauncherWordCommand = new Command(async () =>
        {
            try
            {
                string url = "https://file-examples-com.github.io/uploads/2017/02/file-sample_100kB.docx";

                string path = await DownloadFileAsync(url, "Test", ".docx");

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(path)
                });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "Ok");
            }
        }));

        private ICommand openWordDocCommand;
        public ICommand OpenWordDocCommand => openWordDocCommand ?? (openWordDocCommand = new Command(async () =>
        {
            try
            {
                string url = "https://file-examples-com.github.io/uploads/2017/02/file-sample_100kB.docx";

                string path = await DownloadFileAsync(url, "Test", ".docx");

                ComponentContainer.Current.Resolve<IOpenFileService>().Open("", path);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "Ok");
            }
        }));

        /// <summary>
        /// A simple helper method to download the file for this example
        /// </summary>
        /// <param name="url">Url to file for download</param>
        /// <param name="filename">Filename when file is downloaded</param>
        /// <param name="fileExtension">Extension on file (example: .pdf)</param>
        /// <returns></returns>
        private async Task<string> DownloadFileAsync(string url, string filename, string fileExtension)
        {
            try
            {
                // Ask permission to storage
                var status = await Permissions.RequestAsync<Permissions.StorageWrite>();

                if (status != PermissionStatus.Granted)
                {
                    throw new PermissionException("Denied permission for storage");
                }

                var data = await MakeRequestAsync<byte[]>(url, HttpMethod.Get, null, ParseType.BYTES);

                if (!localFileSystemService.Exists("AppDocs"))
                {
                    localFileSystemService.CreateFolder("AppDocs");
                }

                return localFileSystemService.SaveFile(data, "AppDocs", filename + fileExtension);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private readonly ILocalFileSystemService localFileSystemService;
    }
}
