using System;
using System.Threading.Tasks;
using DeckOfCards.Controls;
using DeckOfCards.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DeckOfCards.Views
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Initialize();

        }

        protected void Initialize()
        {
            ((BaseViewModel)BindingContext).UIBannerRequested += (s, e) => ShowBanner(e);
        }

        public async void ShowBanner(string text)
        {
            try
            {
                var banner = FindByName("BannerView") as Banner;

                if (banner == null) return;

                banner.Text = text;

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await banner.TranslateTo(banner.TranslationX, 0, 350, Easing.SinInOut);
                    await Task.Delay(2000);
                    HideBanner();
                });
            }
            catch (Exception ex)
            {
                HideBanner();
            }
        }

        public async void HideBanner()
        {
            var banner = FindByName("BannerView") as Frame;

            if (banner == null) return;

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await banner.TranslateTo(banner.TranslationX, banner.Height, 350, Easing.SinInOut);
            });
        }
    }
}
