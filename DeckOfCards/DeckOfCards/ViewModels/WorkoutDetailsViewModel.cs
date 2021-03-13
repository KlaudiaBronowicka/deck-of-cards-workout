using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DeckOfCards.Contracts.Services;
using DeckOfCards.Models;
using Xamarin.Forms;

namespace DeckOfCards.ViewModels
{
    public class WorkoutDetailsViewModel : BaseViewModel
    {
        private Workout _workout;
        public Workout Workout
        {
            get => _workout;
            set
            {
                _workout = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<ExerciseItem, int> _finishedExercises;

        public Dictionary<ExerciseItem, int> FinishedExercises
        {
            get => _finishedExercises;
            set
            {
                _finishedExercises = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveCommand => new Command(RemoveItem);

        private readonly IPopupService _popupService;
        private readonly IWorkoutService _workoutService;
        private readonly INavigationService _navigationService;

        public WorkoutDetailsViewModel(IPopupService popupService, IWorkoutService workoutService, INavigationService navigationService)
        {
            _popupService = popupService;
            _workoutService = workoutService;
            _navigationService = navigationService;
        }

        public override Task InitializeAsync(object data)
        {
            Workout = data as Workout;

            var dict = new Dictionary<ExerciseItem, int>();

            foreach(var exerciseEntry in Workout.FinishedExercises)
            {

                dict.Add(Workout.Exercises.Where(x => x.CardSymbol == exerciseEntry.Key).FirstOrDefault(), exerciseEntry.Value);
            }

            FinishedExercises = dict;
            
            return Task.CompletedTask;
        }

        private async void RemoveItem()
        {
            var result = await _popupService.ShowDialog(
                       "Remove workout",
                       "Are you sure you want to remove this workout?",
                       "No",
                       "Yes");

            if (result)
            {
                await _workoutService.RemoveWorkout((int)Workout.Id);

                await _navigationService.NavigateBackAsync();

                MessagingCenter.Send(this, "WorkoutDeleted");

                return;
            }
            
        }
    }
}
