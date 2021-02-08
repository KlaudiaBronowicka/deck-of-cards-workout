using System;
using Plugin.LocalNotification;

namespace DeckOfCards.Services
{
    public class NotificationManager : INotificationManager
    {
        public NotificationManager()
        {
        }

        public event EventHandler NotificationReceived;

        public void CancelNotification(int reminderId)
        {
            NotificationCenter.Current.Cancel(reminderId);
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void ReceiveNotification(string title, string message)
        {
            throw new NotImplementedException();
        }

        public void CancelAll()
        {
            NotificationCenter.Current.CancelAll();
        }

        /// <summary>
        /// Schedule and repeat daily
        /// </summary>
        public void ScheduleRepeating(int reminderId, string title, string message, TimeSpan notifyTime)
        {
            CancelNotification(reminderId);

            var request = new NotificationRequest
            {
                NotificationId = reminderId,
                Title = title,
                Description = message,
                ReturningData = null,
                Repeats = NotificationRepeat.Daily,
                NotifyTime = GetNextDayDateTime(notifyTime),
                Android =
                {
                    IconName = "cards",
                    //AutoCancel = false,
                    
                },
            };

            NotificationCenter.Current.Show(request);
        }



        /// <summary>
        /// Schedule for specific days of the week
        /// </summary>
        public void ScheduleRepeating(int reminderId, string title, string message, TimeSpan notifyTime, bool[] daysOfTheWeek)
        {
            CancelNotification(reminderId);

            for (int i = 0; i < daysOfTheWeek.Length; i++)
            {
                var notificationId = int.Parse($"{reminderId}{i}");

                if (!daysOfTheWeek[i])
                {
                    CancelNotification(notificationId);
                    continue;
                }

                var nextReminderDate = GetNextReminderForDayOfWeek((DayOfWeek)i, notifyTime);

                var request = new NotificationRequest
                {
                    NotificationId = notificationId,
                    Title = title,
                    Description = message,
                    ReturningData = null,
                    Repeats = NotificationRepeat.Weekly,
                    NotifyTime = nextReminderDate,
                    Android =
                    {
                        IconName = "cards",
                        //AutoCancel = false,
                        //Ongoing = true
                    },
                };

                NotificationCenter.Current.Show(request);
            }
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            CancelNotification(999);

            var request = new NotificationRequest
            {
                NotificationId = 999,
                Title = title,
                Description = message,
                ReturningData = null,
                Repeats = NotificationRepeat.No,
                Android =
                {
                    IconName = "cards",
                    //AutoCancel = false,
                    //Ongoing = true
                },
            };

            // if not specified, notification will show immediately.
            if (notifyTime != null)
            {
                request.NotifyTime = notifyTime <= DateTime.Now
                    ? DateTime.Now.AddSeconds(10)
                    : notifyTime;
            }

            NotificationCenter.Current.Show(request);
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
    }
}
