using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DeckOfCards.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {

        private bool _saveUnfinishedWorkouts;
        public bool SaveUnfinishedWorkouts
        {
            get => _saveUnfinishedWorkouts;
            set
            {
                _saveUnfinishedWorkouts = value;
                OnPropertyChanged();
            }
        }

        private bool _animateCardTransitions;
        public bool AnimateCardTransitions
        {
            get => _animateCardTransitions;
            set
            {
                _animateCardTransitions = value;
                OnPropertyChanged();
            }
        }

        private int _scheduledReminders;
        public int ScheduledReminders
        {
            get => _scheduledReminders;
            set
            {
                _scheduledReminders = value;
                OnPropertyChanged();
            }
        }

        public SettingsViewModel()
        {
        }

        public async Task OpenWorkoutRemindersPage()
        {
            await _navigationService.NavigateToAsync<WorkoutRemindersViewModel>();
        }
    }
}
