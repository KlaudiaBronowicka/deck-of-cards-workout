using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DeckOfCards.Contracts.Services;
using System.Threading.Tasks;
using DeckOfCards.Bootstrap;

namespace DeckOfCards.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly INavigationService _navigationService;
        protected readonly IDeckDataService _deckDataService;
        protected readonly IWorkoutService _workoutService;
        protected readonly IPopupService _popupService;

        public BaseViewModel()
        {
            _navigationService = AppContainer.Resolve<INavigationService>();
            _deckDataService = AppContainer.Resolve<IDeckDataService>();
            _popupService = AppContainer.Resolve<IPopupService>();
            _workoutService = AppContainer.Resolve<IWorkoutService>();
        }

        public virtual Task InitializeAsync(object data)
        {
            return Task.FromResult(false);
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
