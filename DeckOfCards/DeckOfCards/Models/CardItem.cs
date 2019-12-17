using System;
using System.Collections.Generic;
using System.Text;

namespace DeckOfCards.Models
{
    public class CardItem
    {
        public readonly CardSymbol Symbol;
        public readonly CardValue Value;


        public CardItem(CardSymbol symbol, CardValue value)
        {
            Symbol = symbol;
            Value = value;
        }
    }
}
