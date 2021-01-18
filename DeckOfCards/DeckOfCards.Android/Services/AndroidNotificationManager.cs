using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using DeckOfCards.Models;
using DeckOfCards.Services;
using Java.Util;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(DeckOfCards.Droid.Services.AndroidNotificationManager))]
namespace DeckOfCards.Droid.Services
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string channelId = "WorkoutReminders";
        const string channelName = "WorkoutReminders";
        const string channelDescription = "Channel for workout reminder notifications.";

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = 0;
        int pendingIntentId = 0;

        NotificationManager _notificationManager;

        public event EventHandler NotificationReceived;

        public static AndroidNotificationManager Instance { get; private set; }

        public void Initialize()
        {
            CreateNotificationChannel();
            Instance = this;
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            if (notifyTime != null)
            {
                Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
                intent.PutExtra(TitleKey, title);
                intent.PutExtra(MessageKey, message);

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.CancelCurrent);
                long triggerTime = GetNotifyTime(notifyTime.Value);
                AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
                alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
            else
            {
                Show(title, message);
            }
        }

        public void ScheduleRepeating(int reminderId, string title, string message, TimeSpan notifyTime)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);


            AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;


            //cancel intent for 'various' reminder
            for (int i = 0; i < 7; i++)
            {
                var intentId = int.Parse($"{reminderId}{i}");
                var potentialPreviousIntent = PendingIntent.GetBroadcast(AndroidApp.Context, intentId, intent, PendingIntentFlags.CancelCurrent);
                alarmManager.Cancel(potentialPreviousIntent);

            }

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.CancelCurrent);

            var nextReminderDate = GetNextDayDateTime(notifyTime);

            long triggerTime = GetNotifyTime(nextReminderDate);


            alarmManager.SetRepeating(AlarmType.RtcWakeup, triggerTime, AlarmManager.IntervalFifteenMinutes / 15, pendingIntent);
        }

        public void ScheduleRepeating(int reminderId, string title, string message, TimeSpan notifyTime, bool[] daysOfTheWeek)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);


            AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;

            //cancel intent for 'everyday' reminder
            var potentialPreviousIntent = PendingIntent.GetBroadcast(AndroidApp.Context, reminderId, intent, PendingIntentFlags.CancelCurrent);
            alarmManager.Cancel(potentialPreviousIntent);

            for (int i = 0; i < daysOfTheWeek.Length; i ++)
            {
                if (!daysOfTheWeek[i]) continue;

                var nextReminderDate = GetNextReminderForDayOfWeek((DayOfWeek)i, notifyTime);

                long triggerTime = GetNotifyTime(nextReminderDate);

                var intentId = int.Parse($"{reminderId}{i}");

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, intentId, intent, PendingIntentFlags.CancelCurrent);

                alarmManager.SetRepeating(AlarmType.RtcWakeup, triggerTime, AlarmManager.IntervalDay * 7, pendingIntent);
            }
        }

        public void CancelNotificaiton(int reminderId)
        {
            Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));

            AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;

            //cancel intent for 'everyday' reminder
            var potentialPreviousIntent = PendingIntent.GetBroadcast(AndroidApp.Context, reminderId, intent, PendingIntentFlags.CancelCurrent);
            alarmManager.Cancel(potentialPreviousIntent);

            //cancel intent for 'various' reminder
            for (int i = 0; i < 7; i++)
            {
                var intentId = int.Parse($"{reminderId}{i}");
                potentialPreviousIntent = PendingIntent.GetBroadcast(AndroidApp.Context, intentId, intent, PendingIntentFlags.CancelCurrent);
                alarmManager.Cancel(potentialPreviousIntent);
            }
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        public void Show(string title, string message)
        {
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.card))
                .SetSmallIcon(Resource.Drawable.card)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            Notification notification = builder.Build();
            _notificationManager.Notify(messageId++, notification);
        }

        void CreateNotificationChannel()
        {
            _notificationManager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                _notificationManager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        private DateTime GetNextReminderForDayOfWeek(DayOfWeek dayOfWeek, TimeSpan time)
        {
            var now = DateTime.Now;

            while (now.DayOfWeek != dayOfWeek)
            {
                now = now.AddDays(1);
            }

            var date = new DateTime(now.Year, now.Month, now.Day, time.Hours, time.Minutes, 0, 0);

            return date;
        }

        private DateTime GetNextDayDateTime(TimeSpan time)
        {
            var now = DateTime.Now;

            var todayReminder = new DateTime(now.Year, now.Month, now.Day, time.Hours, time.Minutes, 0, 0);

            if (todayReminder < now)
            {
                //schedule for tomorrow

                return todayReminder.AddDays(1);
            }
            else
            {
                //schedule for today
                return todayReminder;
            }
        }

        private 

        long GetNotifyTime(DateTime notifyTime)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            double epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            long utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
            return utcAlarmTime; // milliseconds
        }

        
    }
}