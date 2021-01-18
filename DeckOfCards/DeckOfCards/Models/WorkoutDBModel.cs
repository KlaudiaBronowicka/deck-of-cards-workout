using System;
using SQLite;

namespace DeckOfCards.Models
{
    public class WorkoutDBModel
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }

        public string RemainingCards { get; set; }
        public int Seconds { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public bool JokersIncluded { get; set; }
        public string Exercises { get; set; }
        public string FinishedExercises { get; set; }
    }
}
