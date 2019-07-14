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
            ComponentContainer.Current.Register<ILocalizeHelper, LocalizeHelper_Droid>();
			ComponentContainer.Current.Register<IToastHelper, ToastHelper_Droid>();
			ComponentContainer.Current.Register<ILocalFileSystemHelper, LocalFileSystemHelper_Droid>(singelton: true);
        }
    }
}
