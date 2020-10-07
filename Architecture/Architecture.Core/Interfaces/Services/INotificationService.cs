using System;

namespace Architecture.Core
{
    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public interface INotificationService
    {
        void Initialize();
        int ScheduleNotification(string title, string message);
        void ReceiveNotification(string title, string message);

        event EventHandler NotificationReceived;
    }
}
