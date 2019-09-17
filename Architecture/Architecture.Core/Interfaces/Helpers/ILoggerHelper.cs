using System;

namespace Architecture.Core
{
    public interface ILoggerHelper
    {
        void LogException(Exception ex, string className, bool sendToService);
        void ClearLog();
        string GetLog();
    }
}
