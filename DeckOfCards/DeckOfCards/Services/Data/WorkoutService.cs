using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeckOfCards.Contracts.Services;
using DeckOfCards.Models;
using DeckOfCards.Utility;
using System.Linq;

namespace DeckOfCards.Services.Data
{
    public class WorkoutService : IWorkoutService
    {
        DeckOfCardsDB _db;

        public WorkoutService(DeckOfCardsDB db)
        {
            _db = db;
        }

        public async Task<Workout[]> GetAllWorkouts()
        {
            var workouts = await _db.GetAllWorkouts();

            return WorkoutDBListToWorkoutList(workouts).OrderByDescending(x => x.DateStarted).ToArray();
        }

        /// <summary>
        /// Returns last workout if it hasn't been finished yet
        /// </summary>
        /// <returns></returns>
        public async Task<Workout> RestoreLastWorkout()
        {
            var lastWorkout = await _db.GetLastWorkout();

            if (lastWorkout == null) return null;

            if (lastWorkout.DateFinished != null && lastWorkout.DateFinished != default)
            {
                return null;
            }

            var workout = WorkoutDBToWorkout(lastWorkout);

            if (workout.RemainingCards.Count == 0)
            {
                return null;
            }

            return workout;
        }

        public async Task SaveWorkout(Workout workout)
        {
            if (workout == null) return;

            await _db.SaveWorkout(WorkoutToWorkoutDB(workout));
        }

        private List<WorkoutDBModel> WorkoutListToWorkoutDBList(List<Workout> workouts)
        {
            var list = new List<WorkoutDBModel>();
            foreach (var workout in workouts)
            {
                list.Add(WorkoutToWorkoutDB(workout));
            }

            return list;
        }

        private List<Workout> WorkoutDBListToWorkoutList(List<WorkoutDBModel> workouts)
        {
            var list = new List<Workout>();
            foreach (var workout in workouts)
            {
                list.Add(WorkoutDBToWorkout(workout));
            }

            return list;
        }

        private WorkoutDBModel WorkoutToWorkoutDB(Workout workout)
        {
            return new WorkoutDBModel
            {
                Id = workout.Id,
                RemainingCards = Helper.SerializeCardList(workout.RemainingCards),
                Seconds = workout.Seconds,
                DateStarted = workout.DateStarted,
                DateFinished = workout.DateFinished,
                JokersIncluded = workout.JokersIncluded,
                Exercises = Helper.SerializeExerciseList(workout.Exercises)
            };
        }

        private Workout WorkoutDBToWorkout(WorkoutDBModel workout)
        {
            var exercises = Helper.DeserializeIntoExerciseList(workout.Exercises);

            return new Workout
            {
                Id = workout.Id,
                RemainingCards = Helper.DeserializeIntoCardList(workout.RemainingCards, exercises),
                Seconds = workout.Seconds,
                DateStarted = workout.DateStarted,
                DateFinished = workout.DateFinished,
                JokersIncluded = workout.JokersIncluded,
                Exercises = exercises
            };
        }
    }
}
