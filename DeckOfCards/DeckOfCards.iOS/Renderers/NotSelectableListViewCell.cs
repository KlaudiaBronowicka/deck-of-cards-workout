﻿using System;
using CoreGraphics;
using DeckOfCards.iOS.Renderers;
using DeckOfCards.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NotSelectableListViewCell), typeof(NotSelectableListViewCellRenderer))]

namespace DeckOfCards.iOS.Renderers
{
    public class NotSelectableListViewCellRenderer : ViewCellRenderer
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
                _bgView.Layer.BackgroundColor = CGColor.CreateSrgb(0, 0, 0, 0);
            }

            cell.SelectedBackgroundView = _bgView;

            return cell;
            
        }
    }
}
