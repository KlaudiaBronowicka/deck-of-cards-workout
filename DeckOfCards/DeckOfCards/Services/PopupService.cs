using DeckOfCards.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DeckOfCards.Services
{
    public class PopupService : IPopupService
    {
        public void Initialize()
        {

        }
        public Task ShowDialog(string title, string message, string cancel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
