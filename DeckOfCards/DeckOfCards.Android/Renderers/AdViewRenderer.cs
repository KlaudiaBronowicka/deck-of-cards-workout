using Android.Widget;
using Android.Gms.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;

[assembly: ExportRenderer(typeof(DeckOfCards.Controls.AdControlView), typeof(DeckOfCards.Droid.Renderers.AdViewRenderer))]

namespace DeckOfCards.Droid.Renderers
{
    public class AdViewRenderer : ViewRenderer<Controls.AdControlView, AdView>
    {
        public AdViewRenderer(Context context) : base(context) { }

        const string AdUnitId = "ca-app-pub-9447326003867145/6404644839";
        const string TestAdUnitId = "ca-app-pub-3940256099942544/6300978111";

        AdView _adView;

        AdView CreateNativeAdControl()
        {
            if (_adView != null)
                return _adView;

            // This is a string in the Resources/values/strings.xml that I added or you can modify it here. This comes from admob and contains a / in it

            var adParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

            _adView = new AdView(Context)
            {
                AdSize = AdSize.SmartBanner,
                AdUnitId = AdUnitId,
                LayoutParameters = adParams
            };

            

            _adView.LoadAd(new AdRequest
                            .Builder()
                            .Build());

            return _adView;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Controls.AdControlView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(CreateNativeAdControl());
            }
        }
    }
}