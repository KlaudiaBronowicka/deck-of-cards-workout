using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DeckOfCards.Models;
using DeckOfCards.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using DeckOfCards.Contracts.Services;
using DeckOfCards.Services;
using Xamarin.Essentials;
using DeckOfCards.Controls;

namespace DeckOfCards.ViewModels
{
    public class WorkoutRemindersViewModel : BaseViewModel
    {
        private ObservableCollection<WorkoutReminder> _reminders;
        public ObservableCollection<WorkoutReminder> Reminders
        {
            get => _reminders;
            set
            {
                _reminders = value;
                OnPropertyChanged();
            }
        }

        private int _currentlyEditedReminder = -1;

        public ICommand DaysOfTheWeekLabelTappedCommand => new Command<int>(async (int id) => await OpenDayOfTheWeekSelectionPopup(id));
        public ICommand AddCommand => new Command(async () => await AddNewReminder());
        public ICommand ItemLongPressedCommand => new Command<int>(async (int id) => await OpenRemoveReminderPopup(id));

        private IRemindersService _remindersService;
        private INotificationManager _notificationManager;
        private IPopupService _popupService;

        public WorkoutRemindersViewModel(IRemindersService remindersService, INotificationManager notificationManager, IPopupService popupService)
        {
            _remindersService = remindersService;
            _notificationManager = notificationManager;
        }

        public override Task InitializeAsync(object data)
        {
            Reminders = new ObservableCollection<WorkoutReminder>(data as List<WorkoutReminder>);

            return Task.CompletedTask;
        }

        private async Task OpenDayOfTheWeekSelectionPopup(int id)
        {
            var reminder = Reminders.FirstOrDefault(x => x.Id == id);

            if (reminder == null) return;

            _currentlyEditedReminder = id;

            await PopupNavigation.Instance.PushAsync(new DayOfTheWeekSelectionPopup(reminder, async () => await ReminderDaysUpdated()));
        }

        private async Task OpenRemoveReminderPopup(int id)
        {
            HapticFeedback.Perform(HapticFeedbackType.LongPress);

            var reminder = Reminders.FirstOrDefault(x => x.Id == id);

            if (reminder == null) return;

            if ( await _popupService.ShowDialog("Remove reminder", "Are you sure you want to remove this reminder?", "No", "Yes"))
            {
                await RemoveReminder(id);
            }
        }

        private async Task ReminderDaysUpdated()
        {
            var reminder = Reminders.FirstOrDefault(x => x.Id == _currentlyEditedReminder);

            if (reminder == null) return;

            if (reminder.Active)
            {
                ScheduleReminders(reminder);
            }

            await _remindersService.SaveReminder(reminder);

            var reminders = Reminders.ToList();

            Reminders = null;

            Reminders = new ObservableCollection<WorkoutReminder>(reminders);

            _currentlyEditedReminder = -1;
        }

        public async Task ActiveUpdated(int reminderId)
        {
            var reminder = Reminders.FirstOrDefault(x => x.Id == reminderId);

            if (reminder == null) return;

            if (reminder.Active)
            {
                ScheduleReminders(reminder);
            }
            else
            {
                _notificationManager.CancelNotification((int)reminder.Id);
            }

            await _remindersService.SaveReminder(reminder);

        }

        private async Task RemoveReminder(int id)
        {
            await _remindersService.RemoveReminder(id);

            Reminders.Remove(Reminders.FirstOrDefault(x => x.Id == id));

            _notificationManager.CancelNotification(id);
        }

        public async Task TimeUpdated(int reminderId)
        {
            var reminder = Reminders.FirstOrDefault(x => x.Id == reminderId);

            if (reminder == null) return;

            if (reminder.Active)
            {
                ScheduleReminders(reminder);
            }

            await _remindersService.SaveReminder(reminder);

        }

        private void ScheduleReminders(WorkoutReminder reminder)
        {
            if (reminder.Monday && reminder.Tuesday && reminder.Wednesday && reminder.Thursday
                    && reminder.Friday && reminder.Saturday && reminder.Sunday)
            {
                _notificationManager.ScheduleRepeating((int)reminder.Id, "Deck of cards workout", "It's time to exercise!", reminder.Time);
            }
            else
            {
                _notificationManager.ScheduleRepeating((int)reminder.Id, "Deck of cards workout", "It's time to exercise!", reminder.Time, new bool[]
                    {
                        reminder.Sunday,
                        reminder.Monday,
                        reminder.Tuesday,
                        reminder.Wednesday,
                        reminder.Thursday,
                        reminder.Friday,
                        reminder.Saturday
                    });
            }
        }

        private async Task AddNewReminder()
        {
            var newReminder = new WorkoutReminder
            {
                Active = true,
                Monday = true,
                Tuesday = true,
                Wednesday = true,
                Thursday = true,
                Friday = true,
                Saturday = true,
                Sunday = true,
                Time = new TimeSpan(12, 0, 0)
            };

            newReminder = await _remindersService.SaveReminder(newReminder);

            Reminders.Insert(0, newReminder);
        }
    }
}
