using System;
using CoreGraphics;
using DeckOfCards.iOS.Renderers;
using DeckOfCards.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ListViewCell), typeof(ListViewCellRenderer))]

namespace DeckOfCards.iOS.Renderers
{
    public class ListViewCellRenderer : ViewCellRenderer
    {
        private UIView _bgView;

        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);

            cell.BackgroundColor = UIColor.Black;
            cell.TextLabel.TextColor = UIColor.White;

            if (_bgView == null)
            {
                _bgView = new UIView(cell.SelectedBackgroundView.Bounds);
                _bgView.Layer.BackgroundColor = new CGColor(0.137f, 0.192f, 0.259f, 1); // primary dark blue
            }

            cell.SelectedBackgroundView = _bgView;

            return cell;

        }
    }
}
