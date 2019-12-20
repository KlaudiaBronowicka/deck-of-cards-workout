using DeckOfCards.Bootstrap;
using DeckOfCards.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class CardNumberConverter : IValueConverter
    {
        private int _totalCards;

        public CardNumberConverter()
        {
            var deckService = AppContainer.Resolve<IDeckDataService>();
            _totalCards = deckService.GetNumberOfCardsInDeck();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            int cardNumber = System.Convert.ToInt32(value);

            return $"{cardNumber}/{_totalCards}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
