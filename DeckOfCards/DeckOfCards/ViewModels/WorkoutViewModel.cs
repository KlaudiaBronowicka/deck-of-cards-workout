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
    public enum GameState { Default, Running, Paused };

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

        private int _numberOfCards;
        public int NumberOfCards
        {
            get => _numberOfCards;
            set
            {
                _numberOfCards = value;
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

        private GameState _gameState;
        public GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                OnPropertyChanged();
                OnPropertyChanged("IsGamePaused");
                OnPropertyChanged("IsGameRunning");
            }
        }

        public bool IsGamePaused
        {
            get => GameState == GameState.Paused;
        }

        public bool IsGameRunning
        {
            get => GameState == GameState.Running || GameState == GameState.Paused;
        }

        private Random _random;

        public ICommand NextButtonPressedCommand => new Command(async () => await OnNextButtonPressed());
        public ICommand PauseButtonPressedCommand => new Command(OnPauseButtonPressed);
        public ICommand FinishButtonPressedCommand => new Command(FinishGame);

        public override async Task InitializeAsync(object data)
        {
            UpdateDeck();

            NumberOfCards = await _deckDataService.GetNumberOfCardsInDeck();
        }

        public void OnViewAppearing()
        {
            if (IsGameRunning) return;

            Task.Run(() => InitializeAsync(null));
        }

        public void SetupMessageListeners()
        {
            MessagingCenter.Subscribe<EditDeckViewModel>(this, MessagingCenterConstants.ExercisesUpdated, async (sender) => await InitializeAsync(null));
        }

        private async void UpdateDeck()
        {
            Cards = new ObservableCollection<CardItem>(await _deckDataService.GetFullDeck());
        }

        public async Task OnNextButtonPressed()
        {
            switch (GameState)
            {
                case GameState.Paused:
                    ResumeGame();
                    break;
                case GameState.Running:
                    NextCard();
                    break;
                case GameState.Default:
                    await StartGame();
                    break;
            }
        }

        private void OnPauseButtonPressed()
        {
            if (GameState == GameState.Paused) ResumeGame();
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

        private async Task StartGame()
        {
            Cards = new ObservableCollection<CardItem>(await _deckDataService.GetFullDeck());
            CurrentCardIndex = 0;

            GameState = GameState.Running;

            StartTimer();

            NextCard();
        }

        private void ResumeGame()
        {
            GameState = GameState.Running;
            StartTimer();
        }

        private void PauseGame()
        {
            GameState = GameState.Paused;
        }

        private async void FinishGame()
        {
            GameState = GameState.Default;

            var converter = new Converters.SecondsToTimeConverter();
            var time = converter.Convert(Seconds, typeof(string), null, System.Globalization.CultureInfo.InvariantCulture);
            
            await _popupService.ShowDialog("Congratulations!", $"You finished your workout in {time}!", "OK");

            Seconds = 0;
            CurrentCardIndex = 0;
            CurrentCard = null;

            await InitializeAsync(null);
        }

        private int GetRandomCardIndex()
        {
            _random ??= new Random();
            return _random.Next(0, Cards.Count);
        }

        private void StartTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (GameState == GameState.Running)
                    Seconds += 1;

                return GameState == GameState.Running;
            });
        }
    }
}
