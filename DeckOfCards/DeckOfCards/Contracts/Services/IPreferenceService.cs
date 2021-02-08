using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeckOfCards.Contracts.Services
{
    public interface IPreferenceService
    {
        Task<bool> GetPreference(string name);
        Task UpdatePreference(string name, bool value);
        Task<Dictionary<string, bool>> GetAllPreferences();
    }
}
