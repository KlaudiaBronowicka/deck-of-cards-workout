using System;
using DeckOfCards.Services;
using UIKit;

namespace DeckOfCards.iOS.Services
{
    public class ViewLifecycleService : IViewLifecycleService
    {
        public ViewLifecycleService()
        {
        }

        public void ViewLoaded()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                try
                {
                    var statusBarView = new UIView();
                    var statusBarHeight = UIApplication.SharedApplication.StatusBarFrame.Size.Height;
                    statusBarView.Bounds = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, statusBarHeight);
                    statusBarView.BackgroundColor = UIColor.FromRGB(35, 49, 66);
                    UIApplication.SharedApplication.KeyWindow.Subviews[UIApplication.SharedApplication.KeyWindow.Subviews.Length - 1].AddSubview(statusBarView);

                    statusBarView.TranslatesAutoresizingMaskIntoConstraints = false;
                    statusBarView.HeightAnchor.ConstraintEqualTo(UIApplication.SharedApplication.StatusBarFrame.Height).Active = true;
                    statusBarView.WidthAnchor.ConstraintEqualTo(UIApplication.SharedApplication.KeyWindow.WidthAnchor, 1).Active = true;
                    statusBarView.TopAnchor.ConstraintEqualTo(UIApplication.SharedApplication.KeyWindow.TopAnchor, 0).Active = true;
                    statusBarView.LeftAnchor.ConstraintEqualTo(UIApplication.SharedApplication.KeyWindow.LeftAnchor, 0).Active = true;
                }
                catch
                {
                    Console.WriteLine("Failed to change status bar colour");
                }

            }

            UITabBar.Appearance.BackgroundColor = UIColor.Black; // your color
            UITabBar.Appearance.BarTintColor = UIColor.Black; // your color
            UITabBar.Appearance.TintColor = UIColor.Black;
        }

    }
}
