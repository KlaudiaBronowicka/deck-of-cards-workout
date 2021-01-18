using System;
using System.Collections.Generic;
using DeckOfCards.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace DeckOfCards.Views
{
    public partial class DayOfTheWeekSelectionPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private Action _callback;

        public DayOfTheWeekSelectionPopup(WorkoutReminder reminder, Action onClosedCallback)
        {
            InitializeComponent();

            BindingContext = reminder;

            _callback = onClosedCallback;
        }

        async void PopupButton_Clicked(object sender, EventArgs e)
        {
            _callback();

            await PopupNavigation.Instance.PopAsync();
        }

        public void BackButtonPressed()
        {
            _callback();
        }
    }
}
