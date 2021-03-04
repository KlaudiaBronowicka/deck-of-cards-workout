using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DeckOfCards.Controls
{
    public partial class Banner : Frame
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
                                                         propertyName: nameof(Text),
                                                         returnType: typeof(string),
                                                         declaringType: typeof(Banner),
                                                         propertyChanged: TextPropertyChanged);

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (Banner)bindable;
            ((Label)control.FindByName("BannerLabel")).Text = newValue.ToString();
        }

        public Banner()
        {
            InitializeComponent();
        }
    }
}
