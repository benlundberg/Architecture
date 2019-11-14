using Architecture.Core;
using Architecture.Demos;
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
            Current.MainPage = ViewContainer.Current.CreatePage<HomeMasterViewModel>();
            //Current.MainPage = new NavigationPage(ViewContainer.Current.CreatePage<HomeTabbedViewModel>());
        }

        private void Initialize()
        {
            // Initialize database
            DatabaseRepository.Current
                .Init(ComponentContainer.Current.Resolve<ILocalFileSystemService>().GetLocalPath($"{AppConfig.AppName}DB.db3"));

            Bootstrapper.CreateTables();
            Bootstrapper.RegisterViews();

            // TODO: Remove when using template
            Bootstrapper_Demo.Init();
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
