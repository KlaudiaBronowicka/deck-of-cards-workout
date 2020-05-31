using DeckOfCards.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using DeckOfCards.Constants;
using System.Collections.Generic;
using DeckOfCards.Utility;
using System.Linq;

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
        private Workout _currentWorkout;

        public ICommand NextButtonPressedCommand => new Command(async () => await OnNextButtonPressed());
        public ICommand PauseButtonPressedCommand => new Command(OnPauseButtonPressed);
        public ICommand FinishButtonPressedCommand => new Command(FinishGame);


        private bool AskedAboutUnfinishedWorkout;

        public WorkoutViewModel()
        {
            SetupMessageListeners();
        }

        /// <param name="data">True to try to restore previous workout, false otherwise</param>
        /// <returns></returns>
        public override async Task InitializeAsync(object data)
        {

            if (data.GetType() == typeof(bool) && (bool)data && !AskedAboutUnfinishedWorkout)
            {
                var workout = await _workoutService.RestoreLastWorkout();
                if (workout != null)
                {
                    AskedAboutUnfinishedWorkout = true;
                    // previous workout unfinished
                    var result = await _popupService.ShowDialog(
                        "Unfinished workout",
                        "Would you like to continue your previous workout?",
                        "No",
                        "Yes");

                    if (result)
                    {
                        RestoreWorkout(workout);
                        return;
                    }
                }
            }

            UpdateDeck();

            NumberOfCards = await _deckDataService.GetNumberOfCardsInDeck();
        }

        public async void DeckPreferencesChanged()
        {
            // return because InitializeAsync() runs in 'OnAppearing()' so this case will be handled there
            if (GameState != GameState.Running) return;

            var newJokerPrefs = await _deckDataService.GetJokerPreferences();

            // if joker preferences changed
            if (_currentWorkout.JokersIncluded != newJokerPrefs)
            {
                if (newJokerPrefs) // add jokers to the current deck
                {
                    // add empty workouts because they will get updated later on
                    Cards.Add(new CardItem(CardSymbol.Joker, CardValue.Joker, ""));
                    Cards.Add(new CardItem(CardSymbol.Joker, CardValue.Joker, ""));

                }
                else // remove jokers from the current deck
                {
                    var jokerCards = Cards.Where(x => x.Symbol == CardSymbol.Joker).ToList();
                    foreach (var jokerCard in jokerCards)
                    {
                        Cards.Remove(jokerCard);
                        NumberOfCards--;
                    }
                }                    
            }

            // update workout data
            var newExercises = await _deckDataService.GetExercises();

            foreach (var card in Cards)
            {
                card.Exercise = newExercises.Where(x => x.CardSymbol == card.Symbol).FirstOrDefault()?.Name ?? card.Exercise;                
            }

            // update current card
            CurrentCard.Exercise = newExercises.Where(x => x.CardSymbol == CurrentCard.Symbol).FirstOrDefault()?.Name ?? CurrentCard.Exercise;

        }

        private void RestoreWorkout(Workout workout)
        {
            _currentWorkout = workout;
            Cards = new ObservableCollection<CardItem>(workout.RemainingCards);

            NumberOfCards = workout.JokersIncluded ? 54 : 52;

            Seconds = workout.Seconds;

            //TODO: test if not off by 1
            CurrentCardIndex = NumberOfCards - Cards.Count;

            NextCard();

            GameState = GameState.Running;
            GameState = GameState.Paused;
        }

        public void OnViewAppearing()
        {
            if (IsGameRunning) return;

            Task.Run(() => InitializeAsync(true)).ConfigureAwait(true);
        }

        public async Task OnViewDisappearing()
        {
            //await SaveWorkout();
        }

        public void SetupMessageListeners()
        {
            MessagingCenter.Subscribe<EditDeckViewModel>(this, MessagingCenterConstants.ExercisesUpdated, (sender) => DeckPreferencesChanged());
        }

        public async Task SaveWorkout()
        {
            UpdateWorkoutData();

            _currentWorkout = await _workoutService.SaveWorkout(_currentWorkout);
        }

        public void UpdateWorkoutData()
        {
            if (Cards == null) return;

            _currentWorkout.RemainingCards = new List<CardItem>(Cards);

            _currentWorkout.Seconds = Seconds;

            if (Cards.Count == 0)
            {
                _currentWorkout.DateFinished = DateTime.Now;
            }
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

        private void AddCardToFinished()
        {
            if (CurrentCard == null) return;

            if (_currentWorkout.FinishedExercises == null)
            {
                _currentWorkout.FinishedExercises = new Dictionary<CardSymbol, int>();
            }

            var value = CurrentCard.Value == CardValue.Joker ? 1 : Helper.GetNumberForValue(CurrentCard.Value);

            if (!_currentWorkout.FinishedExercises.ContainsKey(CurrentCard.Symbol))
            {
                _currentWorkout.FinishedExercises.Add(CurrentCard.Symbol, value);
            }
            else
            {
                _currentWorkout.FinishedExercises[CurrentCard.Symbol] += value;
            }
        }

        private void NextCard()
        {
            AddCardToFinished();

            if (Cards.Contains(CurrentCard))
            {
                Cards.Remove(CurrentCard);
            }

            if (Cards.Count == 0)
            {
                FinishGame();
                return;
            }

            Task.Run(() => SaveWorkout());

            var index = GetRandomCardIndex();
            CurrentCard = Cards[index];

            CurrentCardIndex++;
        }

        private async Task StartGame()
        {
            Cards = new ObservableCollection<CardItem>(await _deckDataService.GetFullDeck());
            CurrentCardIndex = 0;

            await InitializeNewWorkoutData();

            GameState = GameState.Running;

            StartTimer();

            NextCard();
        }

        private async Task InitializeNewWorkoutData()
        {
            var jokersIncluded = await _deckDataService.GetJokerPreferences();
            var exercises = await _deckDataService.GetExercises();

            _currentWorkout = new Workout
            {
                RemainingCards = new List<CardItem>(Cards),
                DateStarted = DateTime.Now,
                JokersIncluded = jokersIncluded,
                Exercises = exercises
            };
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

            await SaveWorkout();

            var converter = new Converters.SecondsToTimeConverter();
            var time = converter.Convert(Seconds, typeof(string), null, System.Globalization.CultureInfo.InvariantCulture);
            
            await _popupService.ShowDialog("Congratulations!", $"You finished your workout in {time}!", "OK");

            Seconds = 0;
            CurrentCardIndex = 0;
            CurrentCard = null;
            _currentWorkout = null;

            await InitializeAsync(false);
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
