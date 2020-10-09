using Architecture.Core;
using Architecture.UWP.Helpers;

namespace Architecture.UWP
{
    public class Bootstrapper_UWP
    {
        public static void Initialize()
        {
            // Register common types
            Bootstrapper.RegisterTypes();

            // Register device specific types
            RegisterTypes();
        }

        private static void RegisterTypes()
        {
            // Services
            ComponentContainer.Current.Register<ILocalFileSystemService, LocalFileSystemService_UWP>(singelton: true);
            ComponentContainer.Current.Register<IDialogService, DialogService_UWP>(singelton: true);
        }
    }
}
