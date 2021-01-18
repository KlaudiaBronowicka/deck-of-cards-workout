using System;
using System.Collections.Generic;
using DeckOfCards.Models;
using DeckOfCards.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace DeckOfCards.Views
{
    public partial class WorkoutRemindersPage : ContentPage
    {
        private WorkoutRemindersViewModel _vm;

        public WorkoutRemindersPage()
        {
            InitializeComponent();
            _vm = BindingContext as WorkoutRemindersViewModel;
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

        void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var view = sender as View;

            if (!(view.BindingContext is WorkoutReminder reminder)) return;

            _vm.ActiveUpdated((int)reminder.Id);
        }


        void TimePicker_Unfocused(object sender, FocusEventArgs e)
        {
            var view = sender as View;

            if (!(view.BindingContext is WorkoutReminder reminder)) return;

            _vm.ActiveUpdated((int)reminder.Id);
        }
    }
}
