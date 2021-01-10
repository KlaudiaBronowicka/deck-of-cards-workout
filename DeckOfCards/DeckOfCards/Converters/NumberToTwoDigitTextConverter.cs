using System;
using System.Globalization;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class NumberToTwoDigitTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            int number = System.Convert.ToInt32(value);

            return number < 10 ? $"0{number}" : $"{number}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
