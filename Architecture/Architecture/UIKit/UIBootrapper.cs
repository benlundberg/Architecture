using Architecture.UIKit.ViewModels;
using Architecture.UIKit.Views;

namespace Architecture.UIKit
{
    public static class UIBootrapper
    {
        public static void Init()
        {
            ViewContainer.Current.Register<UIKitHomeViewModel, UIKitHomePage>();

            ViewContainer.Current.Register<ArticleBrowserViewModel, ArticleBrowserPage>();
            ViewContainer.Current.Register<ArticleBrowserVariantViewModel, ArticleBrowserVariantPage>();
            ViewContainer.Current.Register<ArticleListViewModel, ArticleListPage>();

            ViewContainer.Current.Register<ListViewModel, ListPage>();
            ViewContainer.Current.Register<CardsListViewModel, CardsListPage>();

            ViewContainer.Current.Register<MessagesViewModel, MessagesPage>();
            ViewContainer.Current.Register<ChatViewModel, ChatPage>();
        }
    }
}
