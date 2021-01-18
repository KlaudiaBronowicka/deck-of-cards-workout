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

        private IRemindersService _remindersService;
        private INotificationManager _notificationManager;

        public WorkoutRemindersViewModel(IRemindersService remindersService)
        {
            _remindersService = remindersService;

            _notificationManager = DependencyService.Get<INotificationManager>();
            _notificationManager.Initialize();
        }

        public override async Task InitializeAsync(object data)
        {
            var reminders = await _remindersService.GetAllReminders();

            Reminders = new ObservableCollection<WorkoutReminder>(reminders);
        }

        private async Task OpenDayOfTheWeekSelectionPopup(int id)
        {
            var reminder = Reminders.FirstOrDefault(x => x.Id == id);

            if (reminder == null) return;

            _currentlyEditedReminder = id;

            await PopupNavigation.Instance.PushAsync(new DayOfTheWeekSelectionPopup(reminder, async () => await ReminderDaysUpdated()));
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
                _notificationManager.CancelNotificaiton((int)reminder.Id);
            }

            await _remindersService.SaveReminder(reminder);

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
