using System;
using System.Collections.Generic;
using DeckOfCards.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace DeckOfCards.Views
{
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel _vm;

        public SettingsPage()
        {
            InitializeComponent();

            _vm = BindingContext as SettingsViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _vm.InitializeAsync(null);
        }

        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var canvas = args.Surface.Canvas;

            var width = canvas.DeviceClipBounds.Width;
            var height = canvas.DeviceClipBounds.Height;

            canvas.Clear();

            using var paint = new SKPaint();

            SKRect rect = new SKRect(0, 0, width, height);

            paint.Shader = SKShader.CreateRadialGradient(
                                new SKPoint(width * 0.9f, height * 0.8f),
                                1.3f * width,
                                new SKColor[] { new SKColor(69, 93, 122), new SKColor(35, 49, 66) },
                                new float[] { 0, 1 },
                                SKShaderTileMode.Mirror);

            // Draw the gradient on the rectangle
            canvas.DrawRect(rect, paint);
        }

        async void WorkoutRemindersTapped(System.Object sender, System.EventArgs e)
        {
            await _vm.OpenWorkoutRemindersPage();
        }
    }
}
