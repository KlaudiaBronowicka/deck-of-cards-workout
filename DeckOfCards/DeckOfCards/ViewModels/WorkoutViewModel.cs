using DeckOfCards.Contracts.Services;
using DeckOfCards.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DeckOfCards.ViewModels
{
    public class WorkoutViewModel : BaseViewModel
    {
        private ObservableCollection<CardItem> _cards;
        public ObservableCollection<CardItem> Cards
        {
            get => _cards;
            set 
            {
                _cards = value;
                OnPropertyChanged();
            }
        }

        public WorkoutViewModel(INavigationService navigationService, IDeckDataService deckDataService) : base(navigationService, deckDataService)
        {
          
        }

        public override Task InitializeAsync(object data)
        {
            Cards = new ObservableCollection<CardItem>(_deckDataService.GetFullDeck());

            return Task.FromResult(true);
        }

    }
}
