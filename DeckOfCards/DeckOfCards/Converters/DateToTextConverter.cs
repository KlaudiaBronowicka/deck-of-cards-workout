using System;
using System.Globalization;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class DateToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(DateTime)) return null;

            var date = (DateTime)value;

            return date.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
