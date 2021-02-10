using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeckOfCards.Contracts.Services;

namespace DeckOfCards.Services.Data
{
    public class PreferenceService : BaseService, IPreferenceService
    {
        public PreferenceService(DeckOfCardsDB db) : base(db)
        {
        }

        public Task<Dictionary<string, bool>> GetAllPreferences()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetPreference(string name, bool defaultValue = false)
        {
            var preference = await _db.GetPreference(name);

            return preference?.Value ?? defaultValue;
        }

        public async Task UpdatePreference(string name, bool value)
        {
            await _db.SavePreference(new Models.PreferencesDBModel
            {
                Name = name,
                Value = value
            });
        }
    }
}
