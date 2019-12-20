using DeckOfCards.Contracts.Services;
using DeckOfCards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DeckOfCards.Services
{
    public class DeckDataService : IDeckDataService
    {
        List<ExerciseItem> _exercises;
        List<CardItem> _deck;

        public DeckDataService()
        {
            InitiateExerciseData();
            InitiateDeckData();
        }

        public string GetExerciseForCardSymbol(CardSymbol symbol)
        {
            return _exercises.Find(x => x.CardSymbol == symbol).Name;
        }

        public List<ExerciseItem> GetExercises()
        {
            return _exercises;
        }

        public List<CardItem> GetFullDeck()
        {
            var cards = new List<CardItem>();

            foreach (var card in _deck)
            {
                cards.Add(new CardItem(card.Symbol, card.Value, GetExerciseForCardSymbol(card.Symbol)));
            }
            
            return cards;
        }

        public int GetNumberOfCardsInDeck()
        {
            return _deck?.Count ?? 0;
        }

        private void InitiateDeckData()
        {
            // use rules like if we should return Joker or not
            var jokerExercise = GetExerciseForCardSymbol(CardSymbol.Joker);
            _deck = new List<CardItem>()
            {
                new CardItem(CardSymbol.Joker, CardValue.Joker, jokerExercise),
                new CardItem(CardSymbol.Joker, CardValue.Joker, jokerExercise)
            };

            for (int i = 0; i < 4; i++)
            {
                var exerciseName = GetExerciseForCardSymbol((CardSymbol)i);
                for (int j = 0; j < 13; j++)
                {
                    _deck.Add(new CardItem((CardSymbol)i, (CardValue)j, exerciseName));
                }
            }
        }

        private void InitiateExerciseData()
        {
            // try to retrieve from storage first
            _exercises = new List<ExerciseItem>
            {
                new ExerciseItem(CardSymbol.Club, "Sit up"),
                new ExerciseItem(CardSymbol.Hearts, "Burpee"),
                new ExerciseItem(CardSymbol.Diamond, "Push up"),
                new ExerciseItem(CardSymbol.Spade, "Squat"),
                new ExerciseItem(CardSymbol.Joker, "5 of each")
            };
        }
    }
}
