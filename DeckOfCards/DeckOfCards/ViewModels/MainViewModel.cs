using DeckOfCards.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private MenuViewModel _menuViewModel;

        public MainViewModel(INavigationService navigationService, MenuViewModel menuViewModel)
            : base(navigationService)
        {
            _menuViewModel = menuViewModel;
          
        }

        public MenuViewModel MenuViewModel
        {
            get => _menuViewModel;
            set
            {
                _menuViewModel = value;
                OnPropertyChanged();
            }
        }

        public override async Task InitializeAsync(object data)
        {
            await Task.WhenAll
            (
                _menuViewModel.InitializeAsync(data),
                _navigationService.NavigateToAsync<WorkoutViewModel>()
            );
        }
    }
}
