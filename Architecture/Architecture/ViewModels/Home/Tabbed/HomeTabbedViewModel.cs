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

            var loggerPage = ViewContainer.Current.CreatePage<LoggerViewModel>();
            loggerPage.Title = Translate("Gen_Log");
            homePage.Children.Add(loggerPage);

            base.OnPageCreated(page);
        }
    }
}
