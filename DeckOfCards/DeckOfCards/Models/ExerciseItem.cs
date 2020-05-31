using System;
using System.Collections.Generic;
using System.Text;

namespace DeckOfCards.Models
{
    public class ExerciseItem
    {
        public CardSymbol CardSymbol { get; }
        public string Name { get; set; }

        public ExerciseItem(CardSymbol symbol, string name)
        {
            CardSymbol = symbol;
            Name = name;
        }
    }
}
