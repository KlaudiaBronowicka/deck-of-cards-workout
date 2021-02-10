using System;
using DeckOfCards.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(DarkHighlightViewCellRenderer))]
namespace DeckOfCards.iOS.Renderers
{
    public class DarkHighlightViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);

            cell.SelectedBackgroundView = new UIView
            {
                BackgroundColor = UIColor.FromRGB(35, 49, 66)
            };

            return cell;
        }
    }
}
