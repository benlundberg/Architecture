using System;
using System.Collections.Generic;

namespace Architecture.Core
{
    public interface IAnalyticsService
    {
        void LogEvent(string eventId);
        void LogEvent(string eventId, string paramName, string value);
        void LogEvent(string eventId, IDictionary<string, string> parameters);
        void LogScreen(string pageName);
        void LogException(Exception ex, IDictionary<string, string> parameters);
    }
}
