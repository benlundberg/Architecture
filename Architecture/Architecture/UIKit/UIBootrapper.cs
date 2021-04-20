using Architecture.UIKit.ViewModels;
using Architecture.UIKit.Views;

namespace Architecture.UIKit
{
    public static class UIBootrapper
    {
        public static void Init()
        {
            ViewContainer.Current.Register<HomeViewModel, UIKitHomePage>();
            ViewContainer.Current.Register<ArticlesBrowserViewModel, Views.Phone.ArticleBrowserPage>();
            ViewContainer.Current.Register<ArticlesBrowserVariantViewModel, Views.Phone.ArticleBrowserVariantPage>();
            ViewContainer.Current.Register<ArticlesListViewModel, Views.Phone.ArticlesListPage>();
            ViewContainer.Current.Register<LoginViewModel, Views.Phone.FullLoginPage>();
            ViewContainer.Current.Register<SignUpViewModel, Views.Phone.FullSignUpPage>();
            ViewContainer.Current.Register<RecoverPasswordViewModel, Views.Phone.RecoverPasswordPage>();
            ViewContainer.Current.Register<TabbedLoginViewModel, Views.Phone.TabbedLoginPage>();
            ViewContainer.Current.Register<SettingsViewModel, SettingsPage>();
            ViewContainer.Current.Register<FeedbackViewModel, FeedbackPage>();
            ViewContainer.Current.Register<DataViewModel, Views.Phone.DataPage>();
        }
    }
}
