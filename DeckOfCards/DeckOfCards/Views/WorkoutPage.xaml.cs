﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using DeckOfCards.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Rg.Plugins.Popup.Services;

namespace DeckOfCards.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutPage : ContentPage
    {
        private View[] _fadeOutOnGamePausedElements;
        private WorkoutViewModel _vm;

        private TutorialPopup _tutorialPopup;

        public WorkoutPage()
        {
            InitializeComponent();

            _vm = (WorkoutViewModel)BindingContext;

            _vm.WorkoutFinished += (s, e) => OnWorkoutFinished(e);

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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!Xamarin.Essentials.Preferences.Get("SeenTutorial", false))
            {
                ShowTutorial();
            }

            _vm.OnViewAppearing();
        }

        protected override async void OnDisappearing()
        {
            await _vm.OnViewDisappearing();
            base.OnDisappearing();

        }

        private void StartCardPulsingAnimation()
        {
            new Animation {
                { 0, 0.5, new Animation (v => CardView.Scale = v, 1, 1.015, Easing.SinInOut) },
                { 0.5, 1, new Animation (v => CardView.Scale = v, 1.015, 1, Easing.SinInOut) }
                }.Commit(this, "PulsingAnimation", 16, 2000, null, null, () => !_vm.IsGameRunning);
        }

        private void CardTappedEvent(object sender, EventArgs e)
        {
            AnimateToNextCard((View)(((View)sender).Parent));
        }

        void CardSwipedEvent(object sender, SwipedEventArgs e)
        {
            AnimateToNextCard((View)(((View)sender).Parent));
        }

        private async void AnimateToNextCard(View card)
        {
            if (!_vm.AnimateCardTransitions)
            {
                await SwitchToNextCard();
                return;
            };

            double position = 1.5 * card.Width;

            // animate off the screen
            await Task.WhenAll(
                card.TranslateTo(-position, 50, 300, Easing.CubicIn),
                card.RotateTo(-50, 300, Easing.CubicIn));

            // switch to next card
            await _vm.OnNextButtonPressed();

            // shift to opposite side of the screen
            await card.RotateTo(30, 0);
            await card.TranslateTo(position, -80, 0);

            // animate into the screen
            await Task.WhenAll(
                card.TranslateTo(0, 0, 200, Easing.CubicOut),
                card.RotateTo(0, 200, Easing.CubicOut));
        }

        private async Task SwitchToNextCard()
        {
            // switch to next card
            await _vm.OnNextButtonPressed();
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
            double opacity = _vm.GameState switch
            {
                GameState.Paused => 0.5,
                GameState.Running => 1,
                _=> 0,
            };

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
                    ResultsLabel.FadeTo(0, 300);
                    break;
                case GameState.Running:
                    ExerciseLabel.FadeTo(1, 300);
                    GameResumeLabel.FadeTo(0, 300);
                    StartGameLabel.FadeTo(0, 300);
                    ResultsLabel.FadeTo(0, 300);
                    break;
                case GameState.Default:
                    ExerciseLabel.FadeTo(0, 300);
                    GameResumeLabel.FadeTo(0, 300);
                    StartGameLabel.FadeTo(0.5, 300);
                    ResultsLabel.FadeTo(0, 300);
                    break;
                case GameState.Finished:
                    ExerciseLabel.FadeTo(0, 300);
                    GameResumeLabel.FadeTo(0, 300);
                    StartGameLabel.FadeTo(0, 300);
                    ResultsLabel.FadeTo(1, 300);
                    break;
            }
        }

        private async void ToggleButtons()
        {
            switch (_vm.GameState)
            {
                case GameState.Paused:
                    break;

                case GameState.Running:

                    if (!Buttons.IsEnabled)
                    {
                        // new game
                        await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            Buttons.IsEnabled = true;
                            Buttons.TranslationX = -Application.Current.MainPage.Width;
                        });

                        await Task.WhenAll(
                            Buttons.TranslateTo(0, 0, 500, Easing.SpringOut),
                            Buttons.FadeTo(1, 500));
                    }
                    else
                    {
                        // resume game
                    }

                    break;

                case GameState.Default:
                default:

                    //animate buttons away
                    await Task.WhenAll(
                        Buttons.TranslateTo(-Application.Current.MainPage.Width, 0, 500, Easing.SinInOut),
                        Buttons.FadeTo(0, 500));

                    Buttons.IsEnabled = false;

                    break;
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var canvas = args.Surface.Canvas;

            var width = canvas.DeviceClipBounds.Width;
            var height = canvas.DeviceClipBounds.Height;

            canvas.Clear();

            using var paint = new SKPaint();

            SKRect rect = new SKRect(0, 0, width, height);

            paint.Shader = SKShader.CreateRadialGradient(
                                new SKPoint(width * 0.2f, height * 0.5f),
                                1.3f * width,
                                new SKColor[] { new SKColor(69, 93, 122), new SKColor(35, 49, 66) },
                                new float[] { 0, 1 },
                                SKShaderTileMode.Mirror);

            // Draw the gradient on the rectangle
            canvas.DrawRect(rect, paint);
        }

        private void HelpButtonClicked(object sender, EventArgs e)
        {
            ShowTutorial();
        }
        
        private async void ShowTutorial()
        {
            _tutorialPopup ??= new TutorialPopup();

            await PopupNavigation.Instance.PushAsync(_tutorialPopup);

            Xamarin.Essentials.Preferences.Set("SeenTutorial", true);

        }

        private void OnWorkoutFinished(string time)
        {
            ResultsLabelTimeSpan.Text = time;

        }
    }
}