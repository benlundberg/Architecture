using System;

namespace Architecture.Core
{
    /// <summary>
    /// A class to log to file and read exceptions
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Logs the exception to a local file and to an analytics service
        /// </summary>
        /// <param name="ex">Exception to log</param>
        /// <param name="className">Name of class where the exception have occurred. Example: GetType().ToString()</param>
        /// <param name="sendToService">Boolean value if logger should also log to a service. (Google Analytics or AppCenter)</param>
        void LogException(Exception ex, string className, bool sendToService);
        
        /// <summary>
        /// Clears the log file
        /// </summary>
        void ClearLog();

        /// <summary>
        /// Method to read the log file
        /// </summary>
        /// <returns>A string value of the log</returns>
        string GetLog();
    }
}
