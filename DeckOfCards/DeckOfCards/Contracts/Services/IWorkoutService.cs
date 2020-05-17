using System;
using System.Threading.Tasks;
using DeckOfCards.Models;

namespace DeckOfCards.Contracts.Services
{
    public interface IWorkoutService
    {
        Task SaveWorkout(Workout workout);

        Task<Workout[]> GetAllWorkouts();

        Task<Workout> RestoreLastWorkout();

        Task RemoveWorkout(int id);
    }
}
