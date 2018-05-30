using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Architecture;
using Architecture.Core;
using Unity;

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
            // Helpers
            ComponentContainer.Current.Register<ILocalFileSystemHelper, LocalFileSystemHelper_UWP>(singelton: true);
        }
    }
}
