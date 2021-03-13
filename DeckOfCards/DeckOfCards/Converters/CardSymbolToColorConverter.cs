using System;
using System.Globalization;
using DeckOfCards.Models;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class CardSymbolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cardSymbol = (CardSymbol)Enum.Parse(typeof(CardSymbol), value.ToString());

            return cardSymbol switch
            {
                CardSymbol.Hearts or CardSymbol.Diamond or CardSymbol.Joker => (Color)Application.Current.Resources["Accent"],
                _ => (Color)Application.Current.Resources["Primary"],
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
