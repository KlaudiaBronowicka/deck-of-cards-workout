using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace DeckOfCards.Views
{
    public partial class TutorialPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public TutorialPopup()
        {
            InitializeComponent();
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
