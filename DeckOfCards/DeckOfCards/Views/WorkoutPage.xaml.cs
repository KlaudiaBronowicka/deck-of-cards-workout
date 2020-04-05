using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeckOfCards.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views;
using SkiaSharp.Views.Forms;

namespace DeckOfCards.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutPage : ContentPage
    {
        private View[] _fadeOutOnGamePausedElements;
        private WorkoutViewModel _vm;

        public WorkoutPage()
        {
            InitializeComponent();
            _vm = (WorkoutViewModel)BindingContext;

            _vm.PropertyChanged += OnViewModelPropertyChanged;

            _fadeOutOnGamePausedElements = new[]
            {
                TopLeftCardSymbol,
                TopRightCardSymbol,
                BottomLeftCardSymbol,
                BottomRightCardSymbol
            };

            StartCardPulsingAnimation();
            
        }

        private async void CardTappedEvent(object sender, EventArgs e)
        {
            AnimateToNextCard((View)sender);
        }


        void CardSwipedEvent(Object sender, SwipedEventArgs e)
        {
            AnimateToNextCard((View)sender);
        }

        private void StartCardPulsingAnimation()
        {
            new Animation {
                { 0, 0.5, new Animation (v => CardView.Scale = v, 1, 1.015, Easing.SinInOut) },
                { 0.5, 1, new Animation (v => CardView.Scale = v, 1.015, 1, Easing.SinInOut) }
                }.Commit(this, "PulsingAnimation", 16, 2000, null, null, () => !_vm.IsGameRunning);

        }

        private async void AnimateToNextCard(View card)
        {
            /*
            if (_vm.GameState != GameState.Running)
            {
                // start game without animation
                _vm.NextButtonPressedCommand.Execute(null);
                return;
            }
            */

            uint transitionTime = 300;
            double displacement = 1.5 * card.Width;

            await Task.WhenAll(
                card.TranslateTo(-displacement, 50, transitionTime, Easing.CubicIn),
                card.RotateTo(-50, transitionTime, Easing.CubicIn)
                );

           await _vm.OnNextButtonPressed();

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
                ToggleButtons();
               
            }
        }

        private void FadeCardSymbols()
        {
            double opacity = 0;

            switch (_vm.GameState)
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
            switch (_vm.GameState)
            {
                case GameState.Paused:
                    ExerciseLabel.FadeTo(0, 300);
                    GameResumeLabel.FadeTo(1, 300);
                    break;
                case GameState.Running:
                    ExerciseLabel.FadeTo(1, 300);
                    GameResumeLabel.FadeTo(0, 300);
                    StartGameLabel.FadeTo(0, 300);
                    break;
                case GameState.Default:
                    ExerciseLabel.FadeTo(0, 300);
                    GameResumeLabel.FadeTo(0, 300);
                    StartGameLabel.FadeTo(0.5, 300);
                    break;
            }
        }

        private async void ToggleButtons()
        {
            switch (_vm.GameState)
            {
                case GameState.Paused:
                    FinishButton.FadeTo(1, 300);
                    FinishButton.TranslateTo(0, 80, 300, Easing.SpringOut);
                    break;
                case GameState.Running:
                    if (!Buttons.IsEnabled)
                    {
                        // new game
                        Buttons.IsEnabled = true;
                        await Buttons.TranslateTo(-Application.Current.MainPage.Width, 0, 0);
                        Buttons.TranslateTo(0, 0, 500, Easing.SpringOut);
                        Buttons.FadeTo(1, 500);
                    }
                    //FinishButton.FadeTo(0, 300);
                    FinishButton.TranslateTo(0, 0, 300, Easing.CubicInOut);
                    break;
                case GameState.Default:
                default:
                    //animate buttons away
                    await Task.WhenAll(
                        Buttons.TranslateTo(-Application.Current.MainPage.Width, 0, 500, Easing.SinInOut),
                        Buttons.FadeTo(0, 500));

                    // if finish button was visible, hide it
                    if (FinishButton.TranslationY != 0)
                    {
                        FinishButton.TranslateTo(0, 0, 300, Easing.CubicInOut);
                    }

                    Buttons.IsEnabled = false;
                    break;
            }
        }



        async void FinishButton_Clicked(Object sender, EventArgs e)
        {
        }

        void PauseButton_Clicked(Object sender, EventArgs e)
        {
        }


        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            var width = canvas.DeviceClipBounds.Width;
            var height = canvas.DeviceClipBounds.Height;


            canvas.Clear();

            using (SKPaint paint = new SKPaint())
            {
                SKRect rect = new SKRect(0, 0, width, height);
                paint.Shader = SKShader.CreateRadialGradient(
                                    new SKPoint(width * 0.2f, height * 0.5f),
                                    1.3f*width,
                                    new SKColor[] { new SKColor(69, 93, 122), new SKColor(35, 49, 66)},
                                    new float[] { 0, 1 },
                                    SKShaderTileMode.Mirror);

                // Draw the gradient on the rectangle
                canvas.DrawRect(rect, paint);
            
            }
        }
    }
}