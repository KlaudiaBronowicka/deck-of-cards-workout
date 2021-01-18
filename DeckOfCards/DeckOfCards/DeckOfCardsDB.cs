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
                    await Database.CreateTablesAsync(CreateFlags.ImplicitPK | CreateFlags.AutoIncPK, typeof(WorkoutDBModel)).ConfigureAwait(false);
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
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(WorkoutReminderDBModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.ImplicitPK | CreateFlags.AutoIncPK, typeof(WorkoutReminderDBModel)).ConfigureAwait(false);
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
                existingItem.Value = preference.Value;

                return await Database.UpdateAsync(existingItem);
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

        public Task<WorkoutDBModel> GetWorkout(DateTime startDate)
        {
            return Database.Table<WorkoutDBModel>().Where(i => i.DateStarted == startDate).FirstOrDefaultAsync();
        }

        public Task<WorkoutDBModel> GetLastWorkout()
        {
            return Database.Table<WorkoutDBModel>().OrderByDescending(x => x.DateStarted).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates workout item if it already exists or creates a new record if it doesn't.
        /// </summary>
        /// <param name="item">Item to update</param>
        /// <returns>Inserted item's id</returns>
        public async Task<int> SaveWorkout(WorkoutDBModel item)
        {
            var existingItem = await GetWorkout(item.DateStarted);

            if (existingItem != null)
            {
                item.Id = existingItem.Id;

                await Database.UpdateAsync(item);

                return (int)item.Id;
            }
            else
            {
                await Database.InsertAsync(item);

                return (int)item.Id;
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

        /// <summary>
        /// Updates reminder item if it already exists or creates a new record if it doesn't.
        /// </summary>
        /// <param name="item">Item to update</param>
        /// <returns>Inserted item's id</returns>
        public async Task<int> SaveReminder(WorkoutReminderDBModel item)
        {
            var existingItem = await GetReminder(item.Id ?? -1);

            if (existingItem != null)
            {
                item.Id = existingItem.Id;

                await Database.UpdateAsync(item);

                return (int)item.Id;
            }
            else
            {
                await Database.InsertAsync(item);

                return (int)item.Id;
            }
        }

        public Task<WorkoutReminderDBModel> GetReminder(int id)
        {
            return Database.Table<WorkoutReminderDBModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<WorkoutReminderDBModel>> GetAllReminders()
        {
            return Database.Table<WorkoutReminderDBModel>().ToListAsync();
        }

        public async Task<int> RemoveReminder(int id)
        {
            var reminder = await GetReminder(id);

            if (reminder == null) return -1;

            return await Database.DeleteAsync(reminder);
        }
    }
}
