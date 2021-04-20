using Architecture.UIKit.Services;
using System.Collections.ObjectModel;

namespace Architecture.UIKit.ViewModels
{
    public class DetailViewModel : BaseViewModel
    {
        public DetailViewModel()
        {
            Comments = new ObservableCollection<ListItemViewModel>(UIKitService.GetListItems(8));
        }

        public ObservableCollection<ListItemViewModel> Comments { get; private set; }
    }
}
