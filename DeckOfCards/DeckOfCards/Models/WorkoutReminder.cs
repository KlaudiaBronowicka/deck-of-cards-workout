using System;
namespace DeckOfCards.Models
{
    public class WorkoutReminder
    {
        public int Id { get; set; }

        public bool Active { get; set; }

        public short Hour { get; set; }
        public short Minute { get; set; }

        public DateTime Time { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

    }
}
