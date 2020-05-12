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

            //InitializeNavigation();
            Current.MainPage = new MainTabbedPage();
        }

        private void InitializeApp()
        {
            AppContainer.RegisterDependencies();

            AppContainer.Resolve<WorkoutViewModel>().SetupMessageListeners();

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override async void OnSleep()
        {
            // Handle when your app sleeps
            await AppContainer.Resolve<WorkoutViewModel>().SaveWorkout();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        
    }
}
