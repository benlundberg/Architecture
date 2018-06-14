using Architecture.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace Architecture.iOS
{
    public class BackgroundHelper_iOS : IBackgroundHelper
    {
        public async Task RunInBackgroundModeAsync(Func<Task> action, string name = "BackgroundTaskName")
        {
            try
            {
                nint taskId = 0;
                bool taskEnded = false;

                taskId = UIApplication.SharedApplication.BeginBackgroundTask(name, () =>
                {
                    // When time is up and task has not finished, call this method to finish the task to prevent the app from being terminated
                    Console.WriteLine($"Background task '{name}' got killed");
                    taskEnded = true;
                    UIApplication.SharedApplication.EndBackgroundTask(taskId);
                });

                await Task.Factory.StartNew(async () =>
                {
                    // Here we run the actual task
                    Console.WriteLine($"Background task '{name}' started");
                    await action();
                    taskEnded = true;
                    UIApplication.SharedApplication.EndBackgroundTask(taskId);
                    Console.WriteLine($"Background task '{name}' finished");
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                await Task.Factory.StartNew(async () =>
                {
                    // Just a method that logs how much time we have remaining. Usually a background task has around 180 seconds to complete. 
                    while (!taskEnded)
                    {
                        Console.WriteLine($"Background task '{name}' time remaining: {UIApplication.SharedApplication.BackgroundTimeRemaining}");
                        await Task.Delay(1000);
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}