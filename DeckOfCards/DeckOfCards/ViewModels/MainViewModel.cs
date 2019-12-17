using DeckOfCards.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public override async Task InitializeAsync(object data)
        {
            await _navigationService.NavigateToAsync<WorkoutViewModel>();
        }
    }
}
