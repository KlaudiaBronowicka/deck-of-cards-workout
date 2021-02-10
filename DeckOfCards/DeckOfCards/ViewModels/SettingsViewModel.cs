using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DeckOfCards.Contracts.Services;
using Xamarin.Forms;
using System.Linq;
using DeckOfCards.Models;
using System.Collections.Generic;
using DeckOfCards.Constants;

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
                UpdateSaveUnfinishedWorkouts();
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
                UpdateAnimateCardTransitions();
            }
        }

        private int _activeReminders;
        public int ActiveReminders
        {
            get => _activeReminders;
            set
            {
                _activeReminders = value;
                OnPropertyChanged();
            }
        }

        private List<WorkoutReminder> _reminders;

        private readonly IRemindersService _remindersService;
        private readonly IPreferenceService _preferenceService;
        private readonly INavigationService _navigationService;

        public SettingsViewModel(IRemindersService remindersService, IPreferenceService preferenceService, INavigationService navigationService)
        {
            _remindersService = remindersService;
            _preferenceService = preferenceService;
            _navigationService = navigationService;
        }

        public override async Task InitializeAsync(object data)
        {
            _reminders = await _remindersService.GetAllReminders();
            ActiveReminders = _reminders.Where(x => x.Active).Count();

            AnimateCardTransitions = await _preferenceService.GetPreference(Constants.Constants.AnimateCardTransitionsPref, true);
            SaveUnfinishedWorkouts = await _preferenceService.GetPreference(Constants.Constants.SaveUnfinishedWorkoutsPref, true);
        }

        public async Task OpenWorkoutRemindersPage()
        {
            await _navigationService.NavigateToAsync<WorkoutRemindersViewModel>(_reminders);
        }

        private async void UpdateAnimateCardTransitions()
        {
            await _preferenceService.UpdatePreference(Constants.Constants.AnimateCardTransitionsPref, AnimateCardTransitions);
        }

        private async void UpdateSaveUnfinishedWorkouts()
        {
            await _preferenceService.UpdatePreference(Constants.Constants.SaveUnfinishedWorkoutsPref, SaveUnfinishedWorkouts);

        }
    }
}
