using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using Architecture.Core;

namespace Architecture.Droid
{
    public class NotificationService_Droid : INotificationService
    {
        public void Initialize()
        {
            CreateNotificationChannel();
        }

        private void CreateNotificationChannel()
        {
            manager = (NotificationManager)global::Android.App.Application.Context.GetSystemService(global::Android.App.Application.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };

                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs
            {
                Title = title,
                Message = message
            };

            NotificationReceived?.Invoke(null, args);
        }

        public int ScheduleNotification(string title, string message)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            messageId++;

            Intent intent = new Intent(global::Android.App.Application.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(global::Android.App.Application.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(global::Android.App.Application.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(global::Android.App.Application.Context.Resources, Resource.Drawable.ic_launcher))
                .SetSmallIcon(Resource.Drawable.ic_launcher)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            var notification = builder.Build();
            manager.Notify(messageId, notification);

            return messageId;
        }

        public event EventHandler NotificationReceived;

        private const string channelId = "default";
        private const string channelName = "Default";
        private const string channelDescription = "The default channel for notifications.";
        private const int pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        private bool channelInitialized = false;
        private int messageId = -1;
        private NotificationManager manager;
    }
}