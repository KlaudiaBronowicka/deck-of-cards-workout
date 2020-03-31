using System;
using System.Globalization;
using DeckOfCards.ViewModels;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class GameStateToTextConverter<T> : IValueConverter
    {
        public T Default { get; set; }
        public T GamePaused { get; set; }
        public T GameRunning{ get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gameState = (GameState)Enum.Parse(typeof(GameState), value.ToString());

            switch (gameState)
            {               
                case GameState.Paused:
                    return GamePaused;
                case GameState.Running:
                    return GameRunning;
                case GameState.Default:
                default:
                    return Default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
