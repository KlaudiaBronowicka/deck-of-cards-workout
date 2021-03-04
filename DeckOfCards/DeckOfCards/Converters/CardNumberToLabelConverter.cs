using System;
using System.Globalization;
using DeckOfCards.Models;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class CardNumberToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cardValue = (CardValue)value;

            switch (cardValue)
            {
                case CardValue.Ace: return "A";
                case CardValue.Two: return "2";
                case CardValue.Three: return "3";
                case CardValue.Four: return "4";
                case CardValue.Five: return "5";
                case CardValue.Six: return "6";
                case CardValue.Seven: return "7";
                case CardValue.Eight: return "8";
                case CardValue.Nine: return "9";
                case CardValue.Ten: return "10";
                case CardValue.Jack: return "J";
                case CardValue.Queen: return "Q";
                case CardValue.King: return "K";
                case CardValue.Joker: return "J\nO\nK\nE\nR";
                default: return string.Empty;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
