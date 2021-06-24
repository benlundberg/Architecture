using Architecture.UIKit.Services;
using System.Collections.ObjectModel;

namespace Architecture.UIKit.ViewModels
{
    public class MessagesViewModel : BaseViewModel
    {
        public MessagesViewModel()
        {
            Messages = new ObservableCollection<MessageItemViewModel>(UIKitService.GetMessages());
        }

        public ObservableCollection<MessageItemViewModel> Messages { get; private set; }
    }
}
