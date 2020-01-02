using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(bool))
                return true;

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(bool))
                return false;

            return !(bool)value;
        }
    }
}
