using Xamarin.Forms;

namespace Architecture
{
    public class HomeTabbedViewModel : BaseViewModel
    {
        public override void OnPageCreated(Page page)
        {
            if (!(page is TabbedPage homePage))
            {
                return;
            }

            // Adding pages to the tabbed page //

            var loginPage = ViewContainer.Current.CreatePage<Demos.LoginViewModel>();
            loginPage.Title = Translate("Gen_Login");
            homePage.Children.Add(loginPage);

            var registerPage = ViewContainer.Current.CreatePage<Demos.SignUpViewModel>();
            registerPage.Title = Translate("Gen_Sign_Up");
            homePage.Children.Add(registerPage);

            var loggerPage = ViewContainer.Current.CreatePage<LoggerViewModel>();
            loggerPage.Title = Translate("Gen_Log");
            homePage.Children.Add(loggerPage);

            base.OnPageCreated(page);
        }
    }
}
