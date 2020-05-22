using System;
using DeckOfCards;
using DeckOfCards.iOS.Renderers;
using DeckOfCards.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DataListView), typeof(DataListRenderer))]
namespace DeckOfCards.iOS.Renderers
{
    public class DataListRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            if (Control != null)
            {
                //Control.ScrollEnabled = false;
                Control.Bounces = false;
            }
        }
    }
}
