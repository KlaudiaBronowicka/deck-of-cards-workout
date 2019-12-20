using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DeckOfCards.Services;
using DeckOfCards.Views;
using DeckOfCards.Contracts.Services;
using System.Threading.Tasks;
using DeckOfCards.ViewModels;
using DeckOfCards.Bootstrap;

namespace DeckOfCards
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            InitializeApp();

            InitializeNavigation();
        }

        private void InitializeApp()
        {
            AppContainer.RegisterDependencies();

            //  put messaging center code here - register for messages
        }

        private async Task InitializeNavigation()
        {
            var navigationService = AppContainer.Resolve<INavigationService>();
            await navigationService.InitializeAsync();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            //TODO: pause/resume workout
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        
    }
}
