using DeckOfCards.Contracts.Services;
using DeckOfCards.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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

        private CardItem _cardItem;
        public CardItem CurrentCard
        {
            get => _cardItem;
            set
            {
                _cardItem = value;
                OnPropertyChanged();
            }
        }

        private int _currentCardIndex;
        public int CurrentCardIndex
        {
            get => _currentCardIndex;
            set
            {
                _currentCardIndex = value;
                OnPropertyChanged();
            }
        }

        private Timer _timer;

        private int _seconds;
        public int Seconds 
        {
            get => _seconds;
            set
            {
                _seconds = value;
                OnPropertyChanged();
            }
        }

        private bool _isGameRunning;
        private Random _random;

        public ICommand ButtonPressedCommand => new Command(OnButtonPressed);

        public override Task InitializeAsync(object data)
        {
            Cards = new ObservableCollection<CardItem>(_deckDataService.GetFullDeck());
            _random = new Random();

            return Task.FromResult(true);
        }

        public void OnButtonPressed()
        {
            if (_isGameRunning)  NextCard();
            else StartGame();
        }

        private void NextCard()
        {
            if (Cards.Contains(CurrentCard))
            {
                Cards.Remove(CurrentCard);
            }

            if (Cards.Count == 0)
            {
                FinishGame();
                return;
            }

            var index = GetRandomCardIndex();
            CurrentCard = Cards[index];

            CurrentCardIndex++;
        }

        private void StartGame()
        {
            Cards = new ObservableCollection<CardItem>(_deckDataService.GetFullDeck());
            CurrentCardIndex = 0;

            _isGameRunning = true;

            StartTimer();

            NextCard();
        }

        private void FinishGame()
        {
            // Display information that you finished

            _isGameRunning = false;
        }

        private int GetRandomCardIndex()
        {
            _random = _random ?? new Random();
            return _random.Next(0, Cards.Count);
        }

        private void StartTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Seconds += 1;
                return _isGameRunning;
            });
        }
    }
}
