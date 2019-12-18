using System;
using System.Collections.Generic;
using System.Text;

namespace DeckOfCards.Models
{
    public class CardItem
    {
        public CardSymbol Symbol { get; }
        public CardValue Value { get; }

        private string _exercise;
        public string Exercise
        {
            get => Value != CardValue.Joker
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

        private static string GetNumberStringForValue(CardValue symbol)
        {
            var number = GetNumberForValue(symbol);
            return number == 0 ? string.Empty : number.ToString();
        }

        private static int GetNumberForValue(CardValue symbol)
        {
            switch(symbol)
            {
                case CardValue.Ace: return 1;
                case CardValue.Two: return 2;
                case CardValue.Three: return 3;
                case CardValue.Four: return 4;
                case CardValue.Five: return 5;
                case CardValue.Six: return 6;
                case CardValue.Seven: return 7;
                case CardValue.Eight: return 8;
                case CardValue.Nine: return 9;
                case CardValue.Ten: return 10;
                case CardValue.Jack: return 11;
                case CardValue.Queen: return 12;
                case CardValue.King: return 13;
                default: return 0;
            }
        }
    }
}
