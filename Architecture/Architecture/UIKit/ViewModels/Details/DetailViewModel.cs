using Architecture.Controls;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class DetailViewModel : BaseViewModel
    {
        public DetailViewModel()
        {

        }

        private ICommand imageCommand;
        public ICommand ImageCommand => imageCommand ?? (imageCommand = new Command(async () =>
        {
            await PopupNavigation.Instance.PushAsync(new ImagePopup(ImageSource.FromUri(new System.Uri("http://clarityapplication.com/dev/images/7.jpg"))));
        }));

        public ObservableCollection<object> TwoItems => new ObservableCollection<object>()
        {
            new object (),
            new object (),
        };

        public ObservableCollection<object> Items => new ObservableCollection<object>()
        {
            new object (),
            new object (),
            new object (),
            new object (),
            new object (),
            new object (),
        };
    }
}
