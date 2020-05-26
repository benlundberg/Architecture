using Architecture.Core;

namespace Architecture.Droid
{
    public class Bootstrapper_Droid
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
            // Helpers
            ComponentContainer.Current.Register<ILocalizeService, LocalizeService_Droid>();
            ComponentContainer.Current.Register<IDialogService, DialogService_Droid>(singelton: true);
            ComponentContainer.Current.Register<ILocalFileSystemService, LocalFileSystemService_Droid>(singelton: true);
        }
    }
}
