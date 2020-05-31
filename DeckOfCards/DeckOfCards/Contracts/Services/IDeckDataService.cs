using DeckOfCards.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeckOfCards.Contracts.Services
{
    public interface IDeckDataService
    {
        Task<List<CardItem>> GetFullDeck();

        Task<List<ExerciseItem>> GetExercises();

        Task<int> GetNumberOfCardsInDeck();

        Task<bool> GetJokerPreferences();

        Task UpdateExerciseData(List<ExerciseItem> newExercises);

        void UpdateJokerPreferences(bool includeJokers);
    }
}
