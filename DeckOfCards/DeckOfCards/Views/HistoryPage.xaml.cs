using System;
using System.Collections.Generic;
using DeckOfCards.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace DeckOfCards.Views
{
    public partial class HistoryPage : ContentPage
    {
        private HistoryViewModel _vm;

        public HistoryPage()
        {
            InitializeComponent();
            _vm = BindingContext as HistoryViewModel;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var canvas = args.Surface.Canvas;

            var width = canvas.DeviceClipBounds.Width;
            var height = canvas.DeviceClipBounds.Height;

            canvas.Clear();

            using var paint = new SKPaint();

            SKRect rect = new SKRect(0, 0, width, height);

            paint.Shader = SKShader.CreateLinearGradient(
                                new SKPoint(width * 0.5f, height),
                                new SKPoint(width * 0.5f, 0),
                                new SKColor[] { new SKColor(69, 93, 122), new SKColor(35, 49, 66) },
                                new float[] { 0, 1 },
                                SKShaderTileMode.Mirror);

            // Draw the gradient on the rectangle
            canvas.DrawRect(rect, paint);
        }

        void WorkoutItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            _vm.WorkoutItemTapped(e.SelectedItemIndex);
            var list = sender as ListView;

            list.SelectedItem = null;
        }

    }
}
