using System;
using System.Collections.Generic;
using System.Text;

namespace DeckOfCards.Models
{
    public class ExerciseItem
    {
        public readonly CardSymbol CardSymbol;
        public readonly string Name;

        public ExerciseItem(CardSymbol symbol, string name)
        {
            CardSymbol = symbol;
            Name = name;
        }
    }
}
