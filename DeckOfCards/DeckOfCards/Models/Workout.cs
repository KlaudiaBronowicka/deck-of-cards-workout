using System;
using System.Collections.Generic;
using SQLite;

namespace DeckOfCards.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public List<CardItem> RemainingCards { get; set; }
        public int Seconds { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public bool JokersIncluded { get; set; }
        public List<ExerciseItem> Exercises { get; set; }
    }
}
