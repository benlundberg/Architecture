namespace Architecture.UIKit.ViewModels
{
    public class ListItemViewModel : BaseItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ImageSource { get; set; }
        public int Rating { get; set; }
        public bool IsFavorite { get; set; }
    }
}
