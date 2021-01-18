using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeckOfCards.Models;

namespace DeckOfCards.Contracts.Services
{
    public interface IRemindersService
    {
        Task<WorkoutReminder> SaveReminder(WorkoutReminder reminder);

        Task<List<WorkoutReminder>> GetAllReminders();

        Task RemoveReminder(int id);
    }
}
