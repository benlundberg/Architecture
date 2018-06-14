using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Architecture.Core;

namespace Architecture.Droid
{
    public class BackgroundHelper_Droid : IBackgroundHelper
    {
        public async Task RunInBackgroundModeAsync(Func<Task> action, string name = "BackgroundTaskName")
        {
            try
            {
                var powerManager = (PowerManager)Application.Context.GetSystemService(Context.PowerService);
                var wakeLock = powerManager.NewWakeLock(WakeLockFlags.Partial, name);

                wakeLock.Acquire();
                bool taskEnded = false;

                await Task.Factory.StartNew(async () =>
                {
                    Console.WriteLine($"Background task '{name}' started");

                    // Run code here
                    await action();
                    Console.WriteLine($"Background task '{name}' finished");

                    // Release wake lock
                    wakeLock.Release();
                    taskEnded = true;
                });

                await Task.Factory.StartNew(async () =>
                {
                    // Stopwatch for debugging how long task is running
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (!taskEnded)
                    {
                        Console.WriteLine($"Background '{name}' task with wakelock still running ({stopwatch.Elapsed.TotalSeconds} seconds)");
                        await Task.Delay(1000);
                    }
                    stopwatch.Stop();
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}