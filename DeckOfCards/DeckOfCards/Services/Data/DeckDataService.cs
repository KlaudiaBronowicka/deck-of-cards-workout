using DeckOfCards.Contracts.Services;
using DeckOfCards.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeckOfCards.Services
{
    public class DeckDataService : BaseService, IDeckDataService
    {
        public DeckDataService(DeckOfCardsDB db) : base(db) {}

        
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

        public async Task UpdateJokerPreferences(bool includeJokers)
        {
            await _db.SavePreference(new PreferencesDBModel { Name = "IncludeJokers", Value = includeJokers });
        }


        public async Task<int> GetNumberOfCardsInDeck()
        {
            var includeJokers = await GetJokerPreferences();

            return includeJokers ? 54 : 52;
        }

        public async Task<bool> GetJokerPreferences()
        {
            var pref = await _db.GetPreference("IncludeJokers");
            return pref == null || pref.Value;
        }

        public async Task UpdateExerciseData(List<ExerciseItem> exercises)
        {
            foreach (var exercise in exercises)
            {
                await _db.SaveExercise(new ExerciseDBModel { CardSymbol = (int)exercise.CardSymbol, Name = exercise.Name });
            }
        }

        //TODO: use automapper everywhere?
        public async Task<List<ExerciseItem>> GetExercises()
        {
            var exercises = new List<ExerciseItem>();

            var exercisesDB = await _db.GetExercises();

            if (exercisesDB.Count == 5)
            {

                for (int i = 0; i < exercisesDB.Count; i++)
                {
                    exercises.Add(new ExerciseItem((CardSymbol)exercisesDB[i].CardSymbol, exercisesDB[i].Name));
                }

                return exercises;
            }

            // save default exercises in db and return them
            exercises = new List<ExerciseItem>
                {
                    new ExerciseItem(CardSymbol.Club, "Sit up"),
                    new ExerciseItem(CardSymbol.Hearts, "Burpee"),
                    new ExerciseItem(CardSymbol.Diamond, "Push up"),
                    new ExerciseItem(CardSymbol.Spade, "Squat"),
                    new ExerciseItem(CardSymbol.Joker, "5 of each")
                };

            foreach (var exercise in exercises)
            {
                await _db.SaveExercise(new ExerciseDBModel { CardSymbol = (int)exercise.CardSymbol, Name = exercise.Name });
            }

            return exercises;
        }

    }
}
