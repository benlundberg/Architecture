using System;
using System.Threading.Tasks;

namespace Architecture.Core
{
    public interface IBackgroundHelper
    {
        Task RunInBackgroundModeAsync(Func<Task> action, string name = "BackgroundTaskName");
    }
}
