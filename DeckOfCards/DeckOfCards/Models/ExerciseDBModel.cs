using System;
using SQLite;

namespace DeckOfCards.Models
{
    public class ExerciseDBModel
    {
        [PrimaryKey]
        public int CardSymbol { get; set; }
        public string Name { get; set; }
    }
}
