using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private View[] _fadeOutOnGamePausedElements;

        public WorkoutPage()
        {
            InitializeComponent();
            ((WorkoutViewModel)BindingContext).PropertyChanged += OnViewModelPropertyChanged;

            _fadeOutOnGamePausedElements = new[]
            {
                TopLeftCardSymbol,
                TopRightCardSymbol,
                BottomLeftCardSymbol,
                BottomRightCardSymbol
            };
            
        }

        private async void CardTappedEvent(object sender, EventArgs e)
        {
            AnimateToNextCard((View)sender);
        }


        void CardSwipedEvent(Object sender, SwipedEventArgs e)
        {
            AnimateToNextCard((View)sender);
        }

        private async void AnimateToNextCard(View card)
        {
            /*
            if (((WorkoutViewModel)BindingContext).GameState != GameState.Running)
            {
                // start game without animation
                ((WorkoutViewModel)BindingContext).NextButtonPressedCommand.Execute(null);
                return;
            }
            */

            uint transitionTime = 300;
            double displacement = 1.5 * card.Width;

            await Task.WhenAll(
                card.TranslateTo(-displacement, 50, transitionTime, Easing.CubicIn),
                card.RotateTo(-50, transitionTime, Easing.CubicIn)
                );

           await ((WorkoutViewModel)BindingContext).OnNextButtonPressed();

            await card.RotateTo(30, 0);
            await card.TranslateTo(displacement, -80, 0);

            await Task.WhenAll(
                card.TranslateTo(0, 0, 200, Easing.CubicOut),
                card.RotateTo(0, 200, Easing.CubicOut)
                );
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameState")
            {
                FadeCardSymbols();
                FadeLabels();
                //ToggleButtons();
               
            }
        }

        private void FadeCardSymbols()
        {
            double opacity = 0;

            switch (((WorkoutViewModel)BindingContext).GameState)
            {
                case GameState.Paused:
                    opacity = 0.5;
                    break;
                case GameState.Running:
                    opacity = 1;
                    break;
                case GameState.Default:
                    opacity = 0;
                    break;
            }

            foreach (var element in _fadeOutOnGamePausedElements)
            {
                element.FadeTo(opacity, 300);
            }
        }

        private void FadeLabels()
        {
            switch (((WorkoutViewModel)BindingContext).GameState)
            {
                case GameState.Paused:
                    ExerciseLabel.FadeTo(0, 300);
                    GameResumeLabel.FadeTo(1, 300);
                    GameStartLabel.FadeTo(0, 300);
                    break;
                case GameState.Running:
                    ExerciseLabel.FadeTo(1, 300);
                    GameResumeLabel.FadeTo(0, 300);
                    GameStartLabel.FadeTo(0, 300);
                    break;
                case GameState.Default:
                    ExerciseLabel.FadeTo(0, 300);
                    GameResumeLabel.FadeTo(0, 300);
                    GameStartLabel.FadeTo(1, 300);
                    break;
            }
        }

        private void ToggleButtons()
        {
            switch (((WorkoutViewModel)BindingContext).GameState)
            {
                case GameState.Paused:
                    FinishButton.FadeTo(1, 300);
                    break;
                case GameState.Running:
                    FinishButton.FadeTo(0, 300);
                    break;
                case GameState.Default:
                default:
                    FinishButton.FadeTo(0, 300);
                    break;
            }
        }

        //TODO: Move to a separate Button effect
        async void FinishButtonPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEnabled")
            {
                var button = sender as Button;

                if (button.IsEnabled)
                {
                    button.FadeTo(1, 300);
                    button.TranslateTo(0, 80, 300, Easing.SpringOut);
                }
                else
                {
                    button.FadeTo(0, 300);
                    button.TranslateTo(0, 0, 300, Easing.CubicInOut);
                }
            }
        }

        //TODO: Move to a separate Button effect
        void PauseButtonPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEnabled")
            {
                var button = sender as Button;

                if (button.IsEnabled)
                {
                    button.FadeTo(1, 300);
                }
                else
                {
                    button.FadeTo(0.5, 300);
                }
            }
        }

    }
}