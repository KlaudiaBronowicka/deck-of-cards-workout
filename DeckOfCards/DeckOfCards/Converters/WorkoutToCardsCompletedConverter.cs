using System;
using System.Globalization;
using DeckOfCards.Models;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class WorkoutToCardsCompletedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(Workout)) return null;

            var workout = value as Workout;

            var totalCards = workout.JokersIncluded ? 54 : 52;

            return $"{totalCards - workout.RemainingCards.Count}/{totalCards}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
