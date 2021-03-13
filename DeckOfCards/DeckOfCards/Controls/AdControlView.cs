using System;
using Xamarin.Forms;

namespace DeckOfCards.Controls
{
    public class AdControlView : Xamarin.Forms.View
    {
        public string AdUnit { get; set; }

        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
                                                         propertyName: nameof(AdUnit),
                                                         returnType: typeof(string),
                                                         declaringType: typeof(AdControlView),
                                                         defaultValue: "",
                                                         defaultBindingMode: BindingMode.TwoWay);
    }
}
