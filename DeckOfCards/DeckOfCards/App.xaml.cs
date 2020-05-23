using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DeckOfCards.Services;
using DeckOfCards.Views;
using DeckOfCards.Contracts.Services;
using System.Threading.Tasks;
using DeckOfCards.ViewModels;
using DeckOfCards.Bootstrap;
using Xamarin.Essentials;

namespace DeckOfCards
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            LoadStyles();


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

        void LoadStyles()
        {
            if (IsASmallDevice())
            {
                //AppResourceDictionary.MergedDictionaries.Add(SmallDeviceStyle.SharedInstance);
                Resources.Add(SmallDeviceStyle.SharedInstance);
            }
            else
            {
                //AppResourceDictionary.MergedDictionaries.Add(GeneralDeviceStyle.SharedInstance);
                Resources.Add(GeneralDeviceStyle.SharedInstance);

            }
        }

        const int SmallWidthResolution = 768;
        const int SmallHeightResolution = 1280;

        public static bool IsASmallDevice()
        {
            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // Width (in pixels)
            var width = mainDisplayInfo.Width;

            // Height (in pixels)
            var height = mainDisplayInfo.Height;
            return (width <= SmallWidthResolution && height <= SmallHeightResolution);
        }
    }
}
