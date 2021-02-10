using System;
using System.Collections.Generic;
using System.Linq;
using Flex;
using Foundation;
using Google.MobileAds;
using UIKit;

namespace DeckOfCards.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            
            Rg.Plugins.Popup.Popup.Init();

            global::Xamarin.Forms.Forms.Init();
            FlexButton.Init();

            // Ask the user for permission to show notifications on iOS 10.0+ at startup.
            // If not asked at startup, user will be asked when showing the first notification.
            Plugin.LocalNotification.NotificationCenter.AskPermission();
            Sharpnado.Shades.iOS.iOSShadowsRenderer.Initialize();

            LoadApplication(new App());

            MobileAds.SharedInstance.Start(null);

            if (!UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                // set status bar color to dark gray
                // works only for ios < 13. Logic for newer versions is implemented with the use of ViewLifecycleService
                UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
                statusBar.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            }


            return base.FinishedLaunching(app, options);
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
        }
    }
}
