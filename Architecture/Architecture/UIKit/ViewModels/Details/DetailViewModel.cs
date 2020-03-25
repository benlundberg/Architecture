using System.Collections.ObjectModel;

namespace Architecture
{
    public class DetailViewModel : BaseViewModel
    {
        public DetailViewModel()
        {

        }

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
