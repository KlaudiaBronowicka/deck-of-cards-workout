using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeckOfCards.Contracts.Services;
using DeckOfCards.Models;

namespace DeckOfCards.Services.Data
{
    public class RemindersService : BaseService, IRemindersService
    {
        public RemindersService(DeckOfCardsDB db) : base(db)
        {
        }

        public async Task<WorkoutReminder> SaveReminder(WorkoutReminder reminder)
        {
            if (reminder == null) return null;

            var id = await _db.SaveReminder(ReminderToReminderDB(reminder));

            reminder.Id = id;

            return reminder;
        }

        public async Task<List<WorkoutReminder>> GetAllReminders()
        {
            var reminders = await _db.GetAllReminders();
            return ReminderDBListToReminderList(reminders);
        }

        public async Task RemoveReminder(int id)
        {
            await _db.RemoveReminder(id);
        }

        private List<WorkoutReminderDBModel> ReminderListToWorkoutDBList(List<WorkoutReminder> workouts)
        {
            var list = new List<WorkoutReminderDBModel>();
            foreach (var workout in workouts)
            {
                list.Add(ReminderToReminderDB(workout));
            }

            return list;
        }

        private List<WorkoutReminder> ReminderDBListToReminderList(List<WorkoutReminderDBModel> workouts)
        {
            var list = new List<WorkoutReminder>();
            foreach (var workout in workouts)
            {
                list.Add(ReminderDBToReminder(workout));
            }

            return list;
        }

        private WorkoutReminderDBModel ReminderToReminderDB(WorkoutReminder workout)
        {
            return new WorkoutReminderDBModel
            {
                Id = workout.Id,
                Time = workout.Time,
                Active = workout.Active,
                Monday = workout.Monday,
                Tuesday = workout.Tuesday,
                Wednesday = workout.Wednesday,
                Thursday = workout.Thursday,
                Friday = workout.Friday,
                Saturday = workout.Saturday,
                Sunday = workout.Sunday
            };
        }

        private WorkoutReminder ReminderDBToReminder(WorkoutReminderDBModel reminder)
        {
            return new WorkoutReminder
            {
                Id = reminder.Id,
                Time = reminder.Time,
                Active = reminder.Active,
                Monday = reminder.Monday,
                Tuesday = reminder.Tuesday,
                Wednesday = reminder.Wednesday,
                Thursday = reminder.Thursday,
                Friday = reminder.Friday,
                Saturday = reminder.Saturday,
                Sunday = reminder.Sunday
            };
        }
    }
}
