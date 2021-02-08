using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace DeckOfCards.Controls
{
    public partial class ConfirmationPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private Action _confirmCallback;
        private Action _cancelCallback;

        public ConfirmationPopup(string title, string message, Action confirmCallback, Action cancelCallback)
        {
            InitializeComponent();

            PopupTitle.Text = title;
            PopupMessage.Text = message;

            _confirmCallback = confirmCallback;
            _cancelCallback = cancelCallback;
        }

        async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            _confirmCallback?.Invoke();

            await PopupNavigation.Instance.PopAsync();
        }

        async void CancelButton_Clicked(object sender, EventArgs e)
        {
            _cancelCallback?.Invoke();

            await PopupNavigation.Instance.PopAsync();
        }

    }
}
