using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeckOfCards.Contracts.Services;
using DeckOfCards.Models;
using DeckOfCards.Utility;
using System.Linq;

namespace DeckOfCards.Services.Data
{
    public class WorkoutService : BaseService, IWorkoutService
    {

        public WorkoutService(DeckOfCardsDB db) : base(db) {}

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

        public async Task<Workout> SaveWorkout(Workout workout)
        {
            if (workout == null) return null;

            var id = await _db.SaveWorkout(WorkoutToWorkoutDB(workout));

            workout.Id = id;

            return workout;
        }

        public async Task RemoveWorkout(int id)
        {
            await _db.DeleteWorkout(id);
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
                Exercises = Helper.SerializeExerciseList(workout.Exercises),
                FinishedExercises = Helper.SerializeFinishedExercises(workout.FinishedExercises)
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
                Exercises = exercises,
                FinishedExercises = Helper.DeserializeIntoFinishedExercises(workout.FinishedExercises)
            };
        }
    }
}
