using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DeckOfCards.Models;
using Xamarin.Forms;

namespace DeckOfCards.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        private ObservableCollection<Workout> _workouts;
        public ObservableCollection<Workout> Workouts
        {
            get => _workouts;
            set
            {
                _workouts = value;
                OnPropertyChanged();
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand => new Command(Refresh);

        public HistoryViewModel()
        {
            Task.Run(() => InitializeAsync(null));
        }

        public async void Refresh()
        {
            IsRefreshing = true;

            await InitializeAsync(null);

            IsRefreshing = false;
        }


        public override async Task InitializeAsync(object data)
        {
            var allWorkouts = await _workoutService.GetAllWorkouts();

            Workouts = new ObservableCollection<Workout>(allWorkouts);
        }


    }
}
