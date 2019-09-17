using Architecture.Core;
using System;
using System.IO;

namespace Architecture
{
    public class LoggerHelper : ILoggerHelper
    {
        public LoggerHelper(ILocalFileSystemHelper localFileSystem)
        {
            this.localFileSystem = localFileSystem;
        }

        public void ClearLog()
        {
            localFileSystem.Delete(LoggerPath);
        }

        public string GetLog()
        {
            return localFileSystem.ReadText(LoggerPath);
        }

        public void LogException(Exception ex, string className, bool sendToService)
        {
            // Print exception in debugger
            ex.Print();

            try
            {
                string logText = DateTime.Now.ToString() + " - " + className + "\n";
                logText += "Message: " + ex.Message + "\n";
                logText += "Stacktrace: " + ex.StackTrace + "\n";
                logText += "Inner exception: " + ex.InnerException?.Message + "\n\n";

                localFileSystem.WriteText(logText, append: true, paths: LoggerPath);

                if (sendToService)
                {
                    // TODO: Send to service, AppCenter?
                }
            }
            catch (Exception e)
            {
                e.Print();
            }
        }

        private string LoggerPath => Path.Combine("LogFile.txt");

        private readonly ILocalFileSystemHelper localFileSystem;
    }
}
