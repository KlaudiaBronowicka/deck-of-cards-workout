using CoreGraphics;
using DeckOfCards.Controls;
using Google.MobileAds;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(AdControlView), typeof(DeckOfCards.iOS.Renderers.AdViewRenderer))]
namespace DeckOfCards.iOS.Renderers
{
    public class AdViewRenderer : ViewRenderer<AdControlView, BannerView>
    {
        private const string TestBannerId = "ca-app-pub-3940256099942544/2934735716";

        private BannerView _adView;

        private BannerView CreateNativeAdControl()
        {
            if (_adView != null)
                return _adView;

            var rootViewController = GetVisibleViewController();

            if (rootViewController == null) return null;

            var bannerId = GetAdUnitId();

            // Setup your BannerView, review AdSizeCons class for more Ad sizes. 
            _adView = new BannerView(size: AdSizeCons.SmartBannerPortrait,
                                           origin: new CGPoint(0, UIScreen.MainScreen.Bounds.Size.Height - AdSizeCons.Banner.Size.Height))
            {
                AdUnitId = bannerId,
                RootViewController = rootViewController
            };

            // Wire AdReceived event to know when the Ad is ready to be displayed
            _adView.AdReceived += (object sender, EventArgs e) =>
            {
                //ad has come in
            };


            _adView.LoadRequest(GetRequest());
            return _adView;
        }

        Request GetRequest()
        {
            var request = Request.GetDefaultRequest();
            // Requests test ads on devices you specify. Your test device ID is printed to the console when
            // an ad request is made. GADBannerView automatically returns test ads when running on a
            // simulator. After you get your device ID, add it here
            //request.TestDevices = new [] { Request.SimulatorId.ToString () };
            return request;
        }

        /// 
        /// Gets the visible view controller.
        /// 
        /// The visible view controller.
        UIViewController GetVisibleViewController()
        {
            var rootController = UIApplication.SharedApplication.KeyWindow?.RootViewController ?? UIApplication.SharedApplication.Windows[0].RootViewController;

            if (rootController.PresentedViewController == null)
                return rootController;

            if (rootController.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)rootController.PresentedViewController).VisibleViewController;
            }

            if (rootController.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;
            }

            return rootController.PresentedViewController;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdControlView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                if (CreateNativeAdControl() == null) return;

                SetNativeControl(_adView);
            }
        }

        private string GetAdUnitId()
        {
            switch (Element.AdUnit)
            {
                case "WorkoutPageBanner": return AdMobConfig.WorkoutPageBannerId;
                case "HistoryPageBanner": return AdMobConfig.HistoryPageBannerId;
                case "SettingsPageBanner": return AdMobConfig.SettingsPageBannerId;
                case "RemindersPageBanner": return AdMobConfig.RemindersPageBannerId;
                default: return AdMobConfig.WorkoutPageBannerId;
            }
        }
    }
}