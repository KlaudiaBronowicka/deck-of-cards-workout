using DeckOfCards.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace DeckOfCards.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public ICommand MenuItemTappedCommand => new Command(OnMenuItemTapped);

        private ObservableCollection<HomeMenuItem> _menuItems;
        public ObservableCollection<HomeMenuItem> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                OnPropertyChanged();
            }
        }

        public MenuViewModel()
        {
            MenuItems = new ObservableCollection<HomeMenuItem>()
            {
                new HomeMenuItem { Id = MenuItemType.EditDeck, Title = "Edit deck" }
            };
        }

        private void OnMenuItemTapped(object menuItemTappedEventArgs)
        {
            var menuItem = (menuItemTappedEventArgs as ItemTappedEventArgs)?.Item as HomeMenuItem;

            if (menuItem == null) return;

            switch (menuItem.Id)
            {
                case MenuItemType.EditDeck:
                    _navigationService.NavigateToAsync<EditDeckViewModel>();
                    break;
            }
        }
    }
}
