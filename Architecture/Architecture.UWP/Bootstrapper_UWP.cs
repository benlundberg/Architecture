using Architecture.Core;

namespace Architecture.UWP
{
    public class Bootstrapper_UWP
    {
        public static void Initialize()
        {
            // Register device specific types
            RegisterTypes();
        }

        private static void RegisterTypes()
        {
            // Helpers
            //ComponentContainer.Current.Register<ILocalizeHelper, LocalizeHelper_UWP>();
            //ComponentContainer.Current.Register<ILocalFileSystemHelper, LocalFileSystemHelper_UWP>(singelton: true);
        }
    }
}
