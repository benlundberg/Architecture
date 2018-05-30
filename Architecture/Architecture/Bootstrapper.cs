using Architecture.Core;

namespace Architecture
{
    public class Bootstrapper
    {
        public static void RegisterTypes()
        {
            // Repositories
            ComponentContainer.Current.Register<IDatabaseRepository, DatabaseRepository>(singelton: true);

            // Services
            ComponentContainer.Current.Register<ILoginService, LoginService>(singelton: true);

            // Helpers
            ComponentContainer.Current.Register<ITranslateHelper, TranslateHelper>();
        }
        
        public static void RegisterViews()
        {
            ViewContainer.Current.Register<LoginViewModel, LoginPage>();
            ViewContainer.Current.Register<RegisterViewModel, RegisterPage>();
            ViewContainer.Current.Register<HomeMasterViewModel, HomeMasterPage>();
            ViewContainer.Current.Register<MasterViewModel, MasterPage>();
            ViewContainer.Current.Register<ListViewModel, ListPage>();
            ViewContainer.Current.Register<MapViewModel, MapPage>();
            ViewContainer.Current.Register<ItemDetailViewModel, ItemDetailPage>();
        }

        public static void CreateTables()
        {
        }
    }
}
