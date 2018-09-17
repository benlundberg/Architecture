using System;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public interface IBackgroundHelper
    {
        /// <summary>
        /// Runs an action in the background even if the app is minimized.
        /// </summary>
        /// <param name="action">Action to run in background</param>
        /// <param name="name">Background task name</param>
        Task RunInBackgroundModeAsync(Func<Task> action, string name = "BackgroundTaskName");
    }
}
