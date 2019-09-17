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

            // Helpers
            ComponentContainer.Current.Register<ITranslateHelper, TranslateHelper>();
            ComponentContainer.Current.Register<INetworkStatusHelper, NetworkStatusHelper>(singelton: true);
            ComponentContainer.Current.Register<ILoggerHelper, LoggerHelper>(singelton: true);
        }

        public static void RegisterViews()
        {
            ViewContainer.Current.Register<HomeMasterViewModel, HomeMasterPage>();
            ViewContainer.Current.Register<MasterViewModel, MasterPage>();
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
