using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class FilesViewModel : BaseViewModel
    {
        private ICommand openPdfCommand;
        public ICommand OpenPdfCommand => openPdfCommand ?? (openPdfCommand = new Command(async () =>
        {
        }));

        private ICommand openWordDocCommand;
        public ICommand OpenWordDocCommand => openWordDocCommand ?? (openWordDocCommand = new Command(async () =>
        {

        }));

        private ICommand openLocalPdfCommand;
        public ICommand OpenLocalPdfCommand => openLocalPdfCommand ?? (openLocalPdfCommand = new Command(async () =>
        {

        }));
    }
}
