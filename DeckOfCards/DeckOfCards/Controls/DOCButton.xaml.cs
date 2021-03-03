using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Sharpnado;
using System.Windows.Input;
using static DeckOfCards.Utility.Helper;

namespace DeckOfCards.Controls
{
    public partial class DOCButton : ContentView
    {
        public event EventHandler<EventArgs> ButtonClicked;

        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public static readonly BindableProperty ButtonCommandProperty = BindableProperty.Create(
                                                         propertyName: nameof(ButtonCommand),
                                                         returnType: typeof(ICommand),
                                                         declaringType: typeof(DOCButton));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
                                                         propertyName: nameof(Text),
                                                         returnType: typeof(string),
                                                         declaringType: typeof(DOCButton),
                                                         propertyChanged: TextPropertyChanged);

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DOCButton)bindable;
            ((Button)control.FindByName("Button")).Text = newValue.ToString();
        }

        public void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Execute(ButtonCommand);

            ButtonClicked?.Invoke(sender, e);
        }

        public DOCButton()
        {
            InitializeComponent();
        }

    }
}
