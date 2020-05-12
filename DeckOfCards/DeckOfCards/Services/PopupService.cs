using DeckOfCards.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace DeckOfCards.Services
{
    public class PopupService : IPopupService
    {
        public void Initialize()
        {

        }

        public Task ShowDialog(string title, string message, string cancel)
        {
            return MainThread.InvokeOnMainThreadAsync(() => Application.Current.MainPage.DisplayAlert(title, message, cancel));
        }

        public Task<bool> ShowDialog(string title, string message, string cancel, string confirm)
        {
            return MainThread.InvokeOnMainThreadAsync(() => Application.Current.MainPage.DisplayAlert(title, message, confirm, cancel));
        }
    }
}
