using DeckOfCards.Contracts.Services;
using DeckOfCards.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using DeckOfCards.Constants;

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
        public bool IsGameRunning
        {
            get => _isGameRunning;
            set
            {
                _isGameRunning = value;
                OnPropertyChanged();
            }
        }

        private bool _isGamePaused;
        public bool IsGamePaused
        {
            get => _isGamePaused;
            set
            {
                _isGamePaused = value;
                OnPropertyChanged();
            }
        }
        
        private Random _random;

        public ICommand NextButtonPressedCommand => new Command(OnNextButtonPressed);
        public ICommand PauseButtonPressedCommand => new Command(OnPauseButtonPressed);
        public ICommand FinishButtonPressedCommand => new Command(FinishGame);

        public override Task InitializeAsync(object data)
        {
            UpdateDeck();

            _random = new Random();

            IsGameRunning = false;

            return Task.FromResult(true);
        }

        public void SetupMessageListeners()
        {
            MessagingCenter.Subscribe<EditDeckViewModel>(this, MessagingCenterConstants.ExercisesUpdated, (sender) => UpdateDeck());
        }

        private async void UpdateDeck()
        {
            Cards = new ObservableCollection<CardItem>(await _deckDataService.GetFullDeck());
        }

        private void OnNextButtonPressed()
        {
            if (IsGamePaused) ResumeGame();

            if (IsGameRunning)  NextCard();
            else StartGame();
        }

        private void OnPauseButtonPressed()
        {
            if (IsGamePaused) ResumeGame();
            else PauseGame();
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

        private async void StartGame()
        {
            Cards = new ObservableCollection<CardItem>(await _deckDataService.GetFullDeck());
            CurrentCardIndex = 0;

            IsGameRunning = true;

            StartTimer();

            NextCard();
        }

        private void ResumeGame()
        {
            IsGamePaused = false;
            StartTimer();
        }

        private void PauseGame()
        {
            IsGamePaused = true;
        }

        private async void FinishGame()
        {
            IsGameRunning = false;

            var converter = new Converters.SecondsToTimeConverter();
            var time = converter.Convert(Seconds, typeof(string), null, System.Globalization.CultureInfo.InvariantCulture);
            
            await _popupService.ShowDialog("Congratulations!", $"You finished your workout in {time}!", "OK");

            Seconds = 0;
            CurrentCardIndex = 0;
            CurrentCard = null;
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
                if (_isGameRunning && !_isGamePaused)
                    Seconds += 1;

                return _isGameRunning && !_isGamePaused;
            });
        }
    }
}
