using Architecture.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture.UIKit
{
    public class DetailViewModel : BaseViewModel
    {
        public DetailViewModel()
        {

        }

        private ICommand imageCommand;
        public ICommand ImageCommand => imageCommand ?? (imageCommand = new Command(async () =>
        {
            await new ImagePopup(ImageSource.FromUri(new System.Uri("https://architectureappimages.blob.core.windows.net/imagecontainer/7.jpg"))).ShowAsync();
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
