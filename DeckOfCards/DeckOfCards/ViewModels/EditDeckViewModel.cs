using DeckOfCards.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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

        public override Task InitializeAsync(object data)
        {
            Exercises = new ObservableCollection<ExerciseItem>(_deckDataService.GetExercises());

            return Task.FromResult(true);
        }
    }
}
