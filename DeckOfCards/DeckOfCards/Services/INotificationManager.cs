using System;
namespace DeckOfCards.Services
{
    //TODO: move to contracts
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();
        void SendNotification(string title, string message, DateTime? notifyTime = null);
        void ReceiveNotification(string title, string message);

        /// <summary>
        /// Schedules a notification at the specified time, repeating every day
        /// </summary>
        void ScheduleRepeating(int reminderId, string title, string message, TimeSpan notifyTime);

        public void ScheduleRepeating(int reminderId, string title, string message, TimeSpan notifyTime, bool[] daysOfTheWeek);

        void CancelNotification(int reminderId);
        void CancelAll();
    }
}
