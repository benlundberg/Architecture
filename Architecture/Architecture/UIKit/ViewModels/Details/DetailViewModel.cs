using System.Collections.ObjectModel;

namespace Architecture
{
    public class DetailViewModel : BaseViewModel
    {
        public DetailViewModel()
        {
            CommentItems = new ObservableCollection<CommentItemViewModel>
            {
                new CommentItemViewModel(),
                new CommentItemViewModel(),
                new CommentItemViewModel()
            };
        }

        public ObservableCollection<CommentItemViewModel> CommentItems { get; set; }
    }

    public class CommentItemViewModel
    {

    }
}
