using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UIKitHomePage : ContentPage
    {
        public UIKitHomePage()
        {
            InitializeComponent();
        }

        // Handles article pages
        private ICommand articleCommand;
        public ICommand ArticleCommand => articleCommand ?? (articleCommand = new Command(async (param) =>
        {
            if (!int.TryParse(param?.ToString(), out int page))
            {
                return;
            }

            switch (page)
            {
                case 1:
                    await Navigation.PushAsync(new ArticleBrowserPage());
                    break;
                case 2:
                    await Navigation.PushAsync(new ArticleClassicPage());
                    break;
                case 3:
                    await Navigation.PushAsync(new ArticleListPage());
                    break;
            }
        }));

        // Handles forms pages
        private ICommand formsCommand;
        public ICommand FormsCommand => formsCommand ?? (formsCommand = new Command(async (param) =>
        {
            if (!int.TryParse(param?.ToString(), out int page))
            {
                return;
            }

            switch (page)
            {
                case 1:
                    await Navigation.PushModalAsync(new FullLoginPage() { BindingContext = new LoginViewModel { Navigation = this.Navigation } });
                    break;
                case 2:
                    await Navigation.PushModalAsync(new FullSignUpPage() { BindingContext = new SignUpViewModel { Navigation = this.Navigation } });
                    break;
                case 3:
                    await Navigation.PushAsync(new TabbedLoginPage() { BindingContext = new TabbedLoginViewModel { Navigation = this.Navigation } });
                    break;
                case 4:
                    await Navigation.PushModalAsync(new ImageLoginPage() { BindingContext = new LoginViewModel { Navigation = this.Navigation } });
                    break;
            }
        }));

        // Handles lists pages
        private ICommand listCommand;
        public ICommand ListCommand => listCommand ?? (listCommand = new Command(async (param) =>
        {
            if (!int.TryParse(param?.ToString(), out int page))
            {
                return;
            }

            switch (page)
            {
                case 1:
                    await Navigation.PushAsync(new ListPage());
                    break;
                case 2:
                    await Navigation.PushAsync(new ListCardPage());
                    break;
                case 3:
                    await Navigation.PushAsync(new ListVariantPage());
                    break;
                case 4:
                    await Navigation.PushAsync(new TimelinePage());
                    break;
                case 5:
                    await Navigation.PushAsync(new SearchPage());
                    break;
            }
        }));

        // Handles details pages
        private ICommand detailCommand;
        public ICommand DetailCommand => detailCommand ?? (detailCommand = new Command(async (param) =>
        {
            if (!int.TryParse(param?.ToString(), out int page))
            {
                return;
            }

            switch (page)
            {
                case 1:
                    await Navigation.PushAsync(new DetailPage());
                    break;
                case 2:
                    await Navigation.PushAsync(new CardDetailPage());
                    break;
                case 3:
                    await Navigation.PushAsync(new VariantDetailPage());
                    break;
            }
        }));
        
        // Handles data pages
        private ICommand dataCommand;
        public ICommand DataCommand => dataCommand ?? (dataCommand = new Command(async (param) =>
        {
            if (!int.TryParse(param?.ToString(), out int page))
            {
                return;
            }

            switch (page)
            {
                case 1:
                    await Navigation.PushAsync(new DataTablePage());
                    break;
                case 2:
                    await Navigation.PushAsync(new TaskOverviewPage());
                    break;
                case 3:
                    await Navigation.PushAsync(new TaskBrowserPage());
                    break;
            }
        })); 
        
        // Handles dashboard pages
        private ICommand dashboardCommand;
        public ICommand DashboardCommand => dashboardCommand ?? (dashboardCommand = new Command(async (param) =>
        {
            if (!int.TryParse(param?.ToString(), out int page))
            {
                return;
            }

            switch (page)
            {
                case 1:
                    await Navigation.PushAsync(new FlatListPage());
                    break;
                case 2:
                    await Navigation.PushAsync(new DashboardImagePage());
                    break;
                case 3:
                    await Navigation.PushAsync(new DashboardCardPage());
                    break;
                case 4:
                    await Navigation.PushAsync(new DashboardMenuPage());
                    break;
            }
        }));
        
        // Handles social pages
        private ICommand socialCommand;
        public ICommand SocialCommand => socialCommand ?? (socialCommand = new Command(async (param) =>
        {
            if (!int.TryParse(param?.ToString(), out int page))
            {
                return;
            }

            switch (page)
            {
                case 1:
                    await Navigation.PushAsync(new SocialCardPage());
                    break;
                case 2:
                    await Navigation.PushAsync(new ContactDetailCardPage());
                    break;
                case 3:
                    await Navigation.PushAsync(new ContactPage());
                    break;
                case 4:
                    await Navigation.PushAsync(new EditContactPage());
                    break;
            }
        }));   
        
        // Handles settings pages
        private ICommand settingsCommand;
        public ICommand SettingsCommand => settingsCommand ?? (settingsCommand = new Command(async (param) =>
        {
            if (!int.TryParse(param?.ToString(), out int page))
            {
                return;
            }

            switch (page)
            {
                case 1:
                    await Navigation.PushAsync(new SettingsPage());
                    break;
                case 2:
                    await Navigation.PushAsync(new AboutPage());
                    break;
            }
        }));   
    }
}