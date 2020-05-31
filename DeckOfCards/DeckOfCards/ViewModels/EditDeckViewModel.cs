using DeckOfCards.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using DeckOfCards.Constants;

namespace DeckOfCards.ViewModels
{
    public class EditDeckViewModel : BaseViewModel
    {
        private ObservableCollection<ExerciseItem> _exercises;
        public ObservableCollection<ExerciseItem> Exercises
        {
            get => _exercises;
            set
            {
                _exercises = value;
                OnPropertyChanged();
            }
        }

        private bool _includeJokers;
        public bool IncludeJokers
        {
            get => _includeJokers;
            set
            {
                _includeJokers = value;
                OnPropertyChanged();                    
            }
        }

        private bool _anyChangesToSave;
        public bool AnyChangesToSave
        {
            get => _anyChangesToSave;
            set
            {
                _anyChangesToSave = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveButtonClicked => new Command(SaveChanges);

        public EditDeckViewModel()
        {
            Task.Run(() => InitializeAsync(null));
        }

        private async void SaveChanges()
        {
            await _deckDataService.UpdateExerciseData(new List<ExerciseItem>(Exercises));
            await _deckDataService.UpdateJokerPreferences(IncludeJokers);

            MessagingCenter.Send(this, MessagingCenterConstants.ExercisesUpdated);

            AnyChangesToSave = false;
        }

        public override async Task InitializeAsync(object data)
        {
            Exercises = new ObservableCollection<ExerciseItem>(await _deckDataService.GetExercises());
            IncludeJokers = await _deckDataService.GetJokerPreferences();

            AnyChangesToSave = false;
        }
        
    }
}
