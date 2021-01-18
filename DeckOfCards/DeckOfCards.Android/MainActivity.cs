using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using DeckOfCards.Droid.Services;
using DeckOfCards.Services;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using DeckOfCards.Views;

namespace DeckOfCards.Droid
{
    [Activity(Label = "DeckOfCards", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            var id = "ca-app-pub-9447326003867145~9473946718";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, id);
            Rg.Plugins.Popup.Popup.Init(this);
            CreateNotificationFromIntent(Intent);
        }

        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(AndroidNotificationManager.TitleKey);
                string message = intent.GetStringExtra(AndroidNotificationManager.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
                var popup = PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1];

                if (popup is DayOfTheWeekSelectionPopup)
                {
                    ((DayOfTheWeekSelectionPopup)popup).BackButtonPressed();
                }

            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
                base.OnBackPressed();
            }
        }
    }
}