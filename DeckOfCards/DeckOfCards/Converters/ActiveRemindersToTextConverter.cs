using System;
using System.Globalization;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class ActiveRemindersToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not int activeReminders) return string.Empty;

            return activeReminders switch
            {
                0 => "No active reminders",
                1 => "1 active reminder",
                _ => $"{activeReminders} active reminders",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}

