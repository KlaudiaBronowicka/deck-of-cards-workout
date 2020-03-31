using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeckOfCards.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DeckOfCards.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutPage : ContentPage
    {
        public WorkoutPage()
        {
            InitializeComponent();
        }

        private async void CardTappedEvent(object sender, EventArgs e)
        {
            if (((WorkoutViewModel)BindingContext).GameState != GameState.Running)
            {
                // start game without animation
                ((WorkoutViewModel)BindingContext).NextButtonPressedCommand.Execute(null);
                return;
            }

            var card = (View)sender;

            uint transitionTime = 300;
            double displacement = 1.5 * card.Width;

            await Task.WhenAll(
                card.TranslateTo(-displacement, 50, transitionTime, Easing.CubicIn),
                card.RotateTo(-50, transitionTime, Easing.CubicIn)
                );

           ((WorkoutViewModel)BindingContext).NextButtonPressedCommand.Execute(null);

           await card.RotateTo(30, 0);
           await card.TranslateTo(displacement, -80, 0);

            await Task.WhenAll(
                card.TranslateTo(0, 0, 200, Easing.CubicOut),
                card.RotateTo(0, 200, Easing.CubicOut)
                );
        }
    }
}