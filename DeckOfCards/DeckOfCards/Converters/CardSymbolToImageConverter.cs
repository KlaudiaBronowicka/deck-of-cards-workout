using DeckOfCards.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace DeckOfCards.Converters
{
    public class CardSymbolToImageConverter : IValueConverter
    {
        private const string _imageAssembly = "DeckOfCards.Assets.Images";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cardSymbol = (CardSymbol)Enum.Parse(typeof(CardSymbol), value.ToString());

            string imageName = string.Empty;

            switch (cardSymbol)
            {
                case CardSymbol.Club:
                    imageName = "club.png";
                    break;
                case CardSymbol.Spade:
                    imageName = "spade.png";
                    break;
                case CardSymbol.Hearts:
                    imageName = "heart.png";
                    break;
                case CardSymbol.Diamond:
                    imageName = "diamond.png";
                    break;
                case CardSymbol.Joker:
                    imageName = "star.png";
                    break;
            }

            return ImageSource.FromResource($"{_imageAssembly}.{imageName}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
