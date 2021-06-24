namespace Architecture.UIKit.ViewModels
{
    public class ArticleItemViewModel : BaseItemViewModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImageSource { get; set; }
        public bool IsFavorite { get; set; }
    }
}
