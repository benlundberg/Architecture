using Architecture.Core;
using Xamarin.Forms;

[assembly: ExportFont("FontAwesome5Brands.otf", Alias = "FontAwesomeBrands")]
[assembly: ExportFont("FontAwesome5Regular.otf", Alias = "FontAwesomeRegular")]
[assembly: ExportFont("FontAwesome5Solid.otf", Alias = "FontAwesomeSolid")]
[assembly: ExportFont("Montserrat-Bold.ttf", Alias = "MontserratBold")]
[assembly: ExportFont("Montserrat-Regular.ttf", Alias = "MontserratRegular")]
[assembly: ExportFont("Montserrat-SemiBold.ttf", Alias = "MontserratSemiBold")]
[assembly: ExportFont("OpenSans-Bold.ttf", Alias = "OpenSansBold")]
[assembly: ExportFont("OpenSans-Regular.ttf", Alias = "OpenSansRegular")]
[assembly: ExportFont("OpenSans-SemiBold.ttf", Alias = "OpenSansSemiBold")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]

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
            Current.MainPage = new NavigationPage(new UIKit.Views.UIKitHomePage());

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

            // TODO: Init analytics
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
