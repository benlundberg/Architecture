using Architecture.Core;
using Xamarin.Forms;

namespace Architecture
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Initialize();

            SetMainPage();
        }

        public static void SetMainPage()
        {
            // TODO: UI Kit home page
            Current.MainPage = new NavigationPage(new ChartsPage()); //UIKitHomePage());

            // Master
            //Current.MainPage = ViewContainer.Current.CreatePage<HomeMasterViewModel>();
            
            // Tabbed
            //Current.MainPage = new NavigationPage(ViewContainer.Current.CreatePage<HomeTabbedViewModel>());
        }

        private void Initialize()
        {
            // Initialize database
            DatabaseRepository.Current
                .Init(ComponentContainer.Current.Resolve<ILocalFileSystemService>().GetLocalPath($"{AppConfig.AppName}DB.db3"));

            Bootstrapper.CreateTables();
            Bootstrapper.RegisterViews();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
