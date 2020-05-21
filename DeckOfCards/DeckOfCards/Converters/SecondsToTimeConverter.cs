using System;
using System.Globalization;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class SecondsToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            int seconds = System.Convert.ToInt32(value);

            if (seconds < 60)
            {
                return seconds < 10 ? $"00:0{seconds}" : $"00:{seconds}";
            }
            else
            {
                int minutes = seconds / 60;
                seconds %= 60;

                if (minutes < 60)
                {
                    var secondsString = seconds < 10 ? $"0{seconds}" : $"{seconds}";
                    return minutes < 10 ? $"0{minutes}:{secondsString}" : $"{minutes}:{secondsString}";
                }
                else
                {
                    int hours = minutes / 60;
                    minutes %= 60;

                    var secondsString = seconds < 10 ? $"0{seconds}" : $"{seconds}";
                    var minutesString = minutes < 10 ? $"0{minutes}" : $"{minutes}";
                    return $"{hours}:{minutesString}:{secondsString}";
                }

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
