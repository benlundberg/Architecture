using System.Threading.Tasks;
using Xamarin.Forms;

namespace Architecture
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Initialize();

            SetMainPage(isLoggedIn: false);
        }

        public static void SetMainPage(bool isLoggedIn)
        {
            if (isLoggedIn)
            {
                Current.MainPage = ViewContainer.Current.CreatePage<HomeMasterViewModel>();
            }
            else
            {
                Current.MainPage = new NavigationPage(ViewContainer.Current.CreatePage<LoginViewModel>());
            }
        }

        private void Initialize()
        {
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
