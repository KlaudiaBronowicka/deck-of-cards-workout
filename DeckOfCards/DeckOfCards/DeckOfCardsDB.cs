using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeckOfCards.Constants;
using DeckOfCards.Models;
using DeckOfCards.Utility;
using SQLite;

namespace DeckOfCards
{
    public class DeckOfCardsDB
    {
        static readonly Lazy<SQLiteAsyncConnection> _lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        });

        static SQLiteAsyncConnection Database => _lazyInitializer.Value;
        static bool initialized = false;

        public DeckOfCardsDB()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(WorkoutDBModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(WorkoutDBModel)).ConfigureAwait(false);
                    initialized = true;
                }
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ExerciseDBModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ExerciseDBModel)).ConfigureAwait(false);
                    initialized = true;
                }
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(PreferencesDBModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(PreferencesDBModel)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task<PreferencesDBModel> GetPreference(string name)
        {
            return Database.Table<PreferencesDBModel>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }

        public async Task<int> SavePreference(PreferencesDBModel preference)
        {
            var existingItem = await GetPreference(preference.Name);

            if (existingItem != null)
            {
                return await Database.UpdateAsync(preference);
            }
            else
            {
                return await Database.InsertAsync(preference);
            }
        }


        public async Task<int> SaveExercise(ExerciseDBModel item)
        {
            var existingItem = await GetExercise(item.CardSymbol);

            if (existingItem != null)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                return await Database.InsertAsync(item);
            }
        }

        //TODO: Split into two repositories for workouts and exercises
        public Task<List<ExerciseDBModel>> GetExercises()
        {
            return Database.Table<ExerciseDBModel>().ToListAsync();
        }

        public Task<ExerciseDBModel> GetExercise(int cardSymbol)
        {
            return Database.Table<ExerciseDBModel>().Where(i => i.CardSymbol == cardSymbol).FirstOrDefaultAsync();
        }


        public Task<List<WorkoutDBModel>> GetAllWorkouts()
        {
            return Database.Table<WorkoutDBModel>().ToListAsync();
        }

        public Task<WorkoutDBModel> GetWorkout(int id)
        {
            return Database.Table<WorkoutDBModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<WorkoutDBModel> GetLastWorkout()
        {
            return Database.Table<WorkoutDBModel>().OrderByDescending(x => x.DateStarted).FirstOrDefaultAsync();
        }

        public async Task<int> SaveWorkout(WorkoutDBModel item)
        {
            var existingItem = await GetWorkout(item.Id);

            if (existingItem != null)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                return await Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteWorkout(WorkoutDBModel item)
        {
            return Database.DeleteAsync(item);
        }

        public async Task<int> DeleteWorkout(int id)
        {
            var workout = await GetWorkout(id);

            if (workout == null) return -1;

            return await Database.DeleteAsync(workout);
        }
    }
}
