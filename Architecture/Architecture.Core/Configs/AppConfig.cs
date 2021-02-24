namespace Architecture.Core
{
    /// <summary>
    /// A class to keep the Apps configuration properties
    /// </summary>
    public class AppConfig
    {
#if PRODUCTION
        public const string AppName = "Architecture";
        public const string AndroidAppIcon = "@drawable/ic_launcher";
#else
        public const string AppName = "ArchitectureTest";
        public const string AndroidAppIcon = "@drawable/icon";

#endif
        /// <summary>
        /// If the Logger Service should log to a local file
        /// </summary>
        public const bool IsFileLogEnabled = true;
    }
}
