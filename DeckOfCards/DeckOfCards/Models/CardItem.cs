using System;
using System.Collections.Generic;
using System.Text;
using static DeckOfCards.Utility.Helper;

namespace DeckOfCards.Models
{
    public class CardItem
    {
        public CardSymbol Symbol { get; }
        public CardValue Value { get; }

        private string _exercise;
        public string Exercise
        {
            get => Value != CardValue.Joker && Value != CardValue.Ace
                    ? $"{GetNumberStringForValue(Value)} {_exercise}s"
                    : $"{GetNumberStringForValue(Value)} {_exercise}";
            set => _exercise = value;
        }


        public CardItem(CardSymbol symbol, CardValue value, string exercise)
        {
            Symbol = symbol;
            Value = value;
            Exercise = exercise;
        }

    }
}
