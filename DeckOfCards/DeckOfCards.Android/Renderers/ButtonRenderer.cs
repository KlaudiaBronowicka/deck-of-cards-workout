

//using Android.Content;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;
//using Android.Views;

//[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(ButtonRenderer))]

//public class ButtonRenderer : Xamarin.Forms.Platform.Android.AppCompat.ButtonRenderer
//{
//    public ButtonRenderer(Context context) : base(context)
//    {
//    }

//    protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
//    {
//        base.OnElementChanged(e);

//        if (e.OldElement != null)
//        {
//            // Cleanup
//        }

//        if (e.NewElement != null)
//        {
//            Control.SetShadowLayer(5, 3, 3, Color.Black.ToAndroid());
//            //Control.Background = Resources.GetDrawable(Resource.Drawable.ButtonShadow);
//            //ViewGroup.SetBackgroundResource(Resource.Drawable.ButtonShadow);
//            //Control.Background = Resources.GetDrawable();
//            //Control.Background = Resources.GetDrawable(Resource.Drawable.button_selector);
//            //ViewGroup.SetBackgroundResource(Resource.Id)
//            ViewGroup.Elevation = 18.0f;
//            ViewGroup.TranslationZ = 20.0f;
//            ViewGroup.SetOutlineAmbientShadowColor(Color.Black.ToAndroid());
//            Control.Elevation = 18.0f;
//            Control.TranslationZ = 20.0f;
//            //Control.viw
//        }
//    }
//}