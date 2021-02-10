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
using Plugin.LocalNotification;

namespace DeckOfCards
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            // Local Notification tap event listener
            NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;

            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);

            LoadStyles();

            InitializeApp();
            //InitializeNavigation();
            Current.MainPage = new MainTabbedPage();
        }

        private void OnLocalNotificationTapped(NotificationTappedEventArgs e)
        {
            // your code goes here
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
            var style = GetDeviceSpecificStyle();

            Resources.Add(style);
        }

        const int SmallWidthResolution = 768;
        const int SmallHeightResolution = 1280;

        const int MidHeightResolution = 1334;

        public static ResourceDictionary GetDeviceSpecificStyle()
        {
            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // Width (in pixels)
            var width = mainDisplayInfo.Width;

            // Height (in pixels)
            var height = mainDisplayInfo.Height;
            if (width <= SmallWidthResolution && height <= SmallHeightResolution)
                return SmallDeviceStyle.SharedInstance;
            else
                return GeneralDeviceStyle.SharedInstance;

        }
    }
}
