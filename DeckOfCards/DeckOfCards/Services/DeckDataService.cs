using DeckOfCards.Contracts.Services;
using DeckOfCards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeckOfCards.Services
{
    public class DeckDataService : IDeckDataService
    {
        List<ExerciseItem> _exercises;

        public DeckDataService()
        {
            InitiateExerciseData();
        }

        public string GetExerciseForCardSymbol(CardSymbol symbol)
        {
            throw new NotImplementedException();
        }

        public List<ExerciseItem> GetExercises()
        {
            return _exercises;
        }

        public List<CardItem> GetFullDeck()
        {
            // use rules like if we should return Joker or not
            var cards = new List<CardItem>()
            {
                new CardItem(CardSymbol.Joker, CardValue.Joker),
                new CardItem(CardSymbol.Joker, CardValue.Joker)
            };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    cards.Add(new CardItem((CardSymbol)i, (CardValue)j));
                }
            }

            return cards;
        }

        private void InitiateExerciseData()
        {
            // try to retrieve from storage first
            _exercises = new List<ExerciseItem>
            {
                new ExerciseItem(CardSymbol.Club, "Sit up"),
                new ExerciseItem(CardSymbol.Hearts, "Burpee"),
                new ExerciseItem(CardSymbol.Diamond, "Push up"),
                new ExerciseItem(CardSymbol.Spade, "Squat")
            };
        }
    }
}
