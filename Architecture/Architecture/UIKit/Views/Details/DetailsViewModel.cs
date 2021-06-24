using Architecture.UIKit.Services;
using System.Collections.ObjectModel;

namespace Architecture.UIKit.ViewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        public DetailsViewModel()
        {
            Comments = new ObservableCollection<ListItemViewModel>(UIKitService.GetListItems(8));
        }

        public ObservableCollection<ListItemViewModel> Comments { get; private set; }
    }
}
