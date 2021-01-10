using System;
using System.Globalization;
using DeckOfCards.Models;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class WorkoutReminderToDaysTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is WorkoutReminder reminder)) return null;

            string text = string.Empty;

            if (reminder.Monday) text += "Monday, ";
            if (reminder.Tuesday) text += "Tuesday, ";
            if (reminder.Wednesday) text += "Wednesday, ";
            if (reminder.Thursday) text += "Thursday, ";
            if (reminder.Friday) text += "Friday, ";
            if (reminder.Saturday) text += "Saturday, ";
            if (reminder.Sunday) text += "Sunday, ";

            return text.Trim().TrimEnd(',');

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
