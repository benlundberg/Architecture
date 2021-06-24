using Architecture.UIKit.Services;
using System.Collections.ObjectModel;

namespace Architecture.UIKit.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        public ChatViewModel()
        {
            Chats = new ObservableCollection<ChatItemViewModel>(UIKitService.GetChats());
        }

        public ObservableCollection<ChatItemViewModel> Chats { get; private set; }
    }
}
