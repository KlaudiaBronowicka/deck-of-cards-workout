using DeckOfCards.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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

        public ICommand SaveButtonClicked => new Command(OnSaveButtonClicked);

        private void OnSaveButtonClicked()
        {
            _deckDataService.UpdateExerciseData(new List<ExerciseItem>(Exercises));
            _navigationService.PopToRootAsync();
        }

        public override Task InitializeAsync(object data)
        {
            Exercises = new ObservableCollection<ExerciseItem>(_deckDataService.GetExercises());

            return Task.FromResult(true);
        }
    }
}
