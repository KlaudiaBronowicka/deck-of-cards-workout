using System;
using System.Collections.ObjectModel;
using DeckOfCards.Models;

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

        public WorkoutRemindersViewModel()
        {
            Reminders = new ObservableCollection<WorkoutReminder>
            {
                new WorkoutReminder
                {
                    Active = true,
                    Monday = true,
                    Tuesday = true,
                    Wednesday = true,
                    Thursday = true,
                    Friday = true,
                    Saturday = true,
                    Sunday = false,
                    Time = DateTime.Now.AddHours(2)
                }
            };

        }
    }
}
