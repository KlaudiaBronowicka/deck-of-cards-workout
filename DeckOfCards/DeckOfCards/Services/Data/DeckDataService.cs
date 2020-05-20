using DeckOfCards.Contracts.Services;
using DeckOfCards.Models;
using System;
using System.Collections.Generic;
using static DeckOfCards.Constants.CacheNameConstants;
using Akavache;
using System.Threading.Tasks;

namespace DeckOfCards.Services
{
    public class DeckDataService : BaseService, IDeckDataService
    {
        public async Task<List<ExerciseItem>> GetExercises() => 
            await GetExerciseDataFromCache() ??
            new List<ExerciseItem>
            {
                new ExerciseItem(CardSymbol.Club, "Sit up"),
                new ExerciseItem(CardSymbol.Hearts, "Burpee"),
                new ExerciseItem(CardSymbol.Diamond, "Push up"),
                new ExerciseItem(CardSymbol.Spade, "Squat"),
                new ExerciseItem(CardSymbol.Joker, "5 of each")
            };

        public async Task<List<CardItem>> GetFullDeck()
        {
            var exercises = await GetExercises();

            var deck = new List<CardItem>();

            // use rules like if we should return Joker or not
            var includeJokers = await GetJokerPreferences();

            if (includeJokers)
            {
                var jokerExercise = exercises.Find(x => x.CardSymbol == CardSymbol.Joker)?.Name;

                deck.Add(new CardItem(CardSymbol.Joker, CardValue.Joker, jokerExercise));
                deck.Add(new CardItem(CardSymbol.Joker, CardValue.Joker, jokerExercise));
            }

            for (int i = 0; i < 4; i++)
            {
                var exerciseName = exercises.Find(x => x.CardSymbol == (CardSymbol)i)?.Name;

                for (int j = 0; j < 13; j++)
                {
                    deck.Add(new CardItem((CardSymbol)i, (CardValue)j, exerciseName));
                }
            }

            return deck;
        }

        public void UpdateJokerPreferences(bool includeJokers)
        {
            Cache.InsertObject("IncludeJokers", includeJokers);
        }


        public async Task<int> GetNumberOfCardsInDeck()
        {
            var includeJokers = await GetJokerPreferences();

            return includeJokers ? 54 : 52;
        }

        public async Task<bool> GetJokerPreferences()
        {
            return await GetFromCache<bool?>("IncludeJokers") ?? true;
        }

        public void UpdateExerciseData(List<ExerciseItem> newExercises)
        {
            SaveExerciseDataToCache(newExercises);
        }

        private void SaveExerciseDataToCache(List<ExerciseItem> exercises)
        {
            foreach (var exercise in exercises)
            {
                Cache.InsertObject($"ExerciseNames{(int)exercise.CardSymbol}", exercise.Name);
            }
        }

        private async Task<List<ExerciseItem>> GetExerciseDataFromCache()
        {
            var exercises = new List<ExerciseItem>();

            for (int i = 0; i < 5; i++)
            {
                var exerciseName = await GetFromCache<string>($"{ExerciseNames}{i}");
                if (string.IsNullOrEmpty(exerciseName)) return null;
                exercises.Add(new ExerciseItem((CardSymbol)i, exerciseName));
            }

            return exercises;
        }
    }
}
