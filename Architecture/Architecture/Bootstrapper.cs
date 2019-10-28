using Architecture.Core;
using System;
using System.Collections.Generic;

namespace Architecture
{
    public class Bootstrapper
    {
        public static void RegisterTypes()
        {
            // Repositories
            ComponentContainer.Current.Register<IDatabaseRepository, DatabaseRepository>(singelton: true);

            // Services
            ComponentContainer.Current.Register<ITranslateService, TranslateService>();
            ComponentContainer.Current.Register<INetworkStatusService, NetworkStatusService>(singelton: true);
            ComponentContainer.Current.Register<ILoggerService, LoggerService>(singelton: true);
        }

        public static void RegisterViews()
        {
            ViewContainer.Current.Register<HomeMasterViewModel, HomeMasterPage>();
            ViewContainer.Current.Register<MasterViewModel, MasterPage>();
            ViewContainer.Current.Register<HomeTabbedViewModel, HomeTabbedPage>();
            ViewContainer.Current.Register<LoggerViewModel, LoggerPage>();
        }

        public static void CreateTables()
        {
            ComponentContainer.Current.Resolve<IDatabaseRepository>().CreateTablesAsync(new List<Type>()
            {
            });
        }
    }
}
