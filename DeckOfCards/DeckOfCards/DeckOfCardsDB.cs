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
            }
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
