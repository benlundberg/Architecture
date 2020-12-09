using Architecture.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace Architecture
{
    public class LoggerService : ILoggerService
    {
        public LoggerService(ILocalFileSystemService localFileSystem)
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
                // Build a log post for the text file
                string logText = DateTime.Now.ToString() + " - " + className + "\n";
                logText += "Message: " + ex.Message + "\n";
                logText += "Stacktrace: " + ex.StackTrace + "\n";
                logText += "Inner exception: " + ex.InnerException?.Message + "\n\n";

                localFileSystem.WriteText(logText, append: true, paths: LoggerPath);

                if (sendToService)
                {
                    // TODO: Send to service
                }
            }
            catch (Exception e)
            {
                e.Print();
            }
        }

        private string LoggerPath => Path.Combine("LogFile.txt");

        private readonly ILocalFileSystemService localFileSystem;
    }
}
