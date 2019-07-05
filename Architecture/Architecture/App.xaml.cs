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
        }

        private void Initialize()
        {
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
