using SQLite;

namespace DeckOfCards.Models
{
    public class PreferencesDBModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Value { get; set; }
    }
}
